using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Towers.Claims;
using Towers.Claims.FeedDefinitions;
using Towers.Claims.FeedProcessing;

namespace Towers.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string path;
            while ((path = CaptureFeedPath(args)) == null)
            {
            }

            const char delimiter = ',';

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var parser = new DelimiterSeparatedFieldParser(delimiter);

            TriangleDimensions largestTriangle;
            using( var inputFileReader = new StreamReader(path) )
            {
                var collector = new ErrorCollector();

                WriteProgress("Processing file (this may take some time)...");

                var columnReader = new ColumnReader(inputFileReader, parser);
                var dimensionReader = new Reader<TriangleFeedYearExtract>(columnReader, collector);
                var largestTriangleCalculator = new LargestTriangleCalculator(dimensionReader.Read);
                largestTriangle = largestTriangleCalculator.Calculate();
                ReportAndQuitIfErrors(collector, path, "triangle_dimension_calculation_errors.txt");
            }

            using( var inputFileReader = new StreamReader(path) )
            {
                var collector = new ErrorCollector();

                using( var outputFileWriter = new StreamWriter(BuildPathFromInputPath(path, "output.csv")) )
                {
                    var columnReader = new ColumnReader(inputFileReader, parser);
                    var paymentRecordReader = new Reader<TriangleFeedFullDataExtract>(columnReader, collector);
                    var triangleBuilder = new TriangleBuilder(paymentRecordReader, collector);
                    Func<ClaimTriangle, string[]> triangleConverter = triangle => triangle.Accumulate().Flatten(largestTriangle);
                    var header = string.Format("{0}, {1}", largestTriangle.OriginYear, largestTriangle.DevelopmentYears);
                    var writer = new ColumnWriter<ClaimTriangle>(triangleBuilder.BuildNext, triangleConverter, header, outputFileWriter, delimiter);
                    writer.Write();   
                }
                
                ReportAndQuitIfErrors(collector, path, "payment_record_errors.txt");
            }
            

            stopwatch.Stop();


            WriteProgress("Processing took: {0}", stopwatch.Elapsed);
            System.Console.WriteLine("Press any key to quit...");
            System.Console.ReadKey();
        }

        private static void ReportAndQuitIfErrors(ErrorCollector collector, string inputFilePath, string errorFilename)
        {
            if (collector.ContainsErrors) // so check for errors now
            {
                collector.WriteTo(BuildPathFromInputPath(inputFilePath, errorFilename));
                System.Console.WriteLine(
                    "Errors detected in input file. Please check the error report and retry once you have figured it out.");
                System.Console.WriteLine("Press any key to quit...");
                System.Console.ReadKey();
                Environment.Exit((int) ExitCode.InvalidInputFile);
            }
        }

        private static string BuildPathFromInputPath(string path, string fileName)
        {
            string outputPath = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(fileName), DateTime.Now.ToString("yyyyMMdd"), Path.GetExtension(fileName));
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName == null) return outputPath;
            return Path.Combine(directoryName, outputPath);
        }

        private static string CaptureFeedPath(string[] args)
        {
            string path;
            if (args.Count() != 2)
            {
                System.Console.WriteLine("Please enter the path to the incremental claim feed (or enter 'q' to quit):");
                path = System.Console.ReadLine();
                if (path == "q") Environment.Exit((int)ExitCode.Success);
            }
            else
            {
                path = args[1];
            }

            System.Console.WriteLine("Checking for file: {0}", path);

            if (!File.Exists(path))
            {
                System.Console.WriteLine("Feed does not exist!");
                return null;
            }

            return path;
        }

        private static void WriteProgress(string message, params object[] args)
        {
            System.Console.SetCursorPosition(0, System.Console.CursorTop-1);
            System.Console.Write(string.Format("{0,-70}", message), args);
            System.Console.SetCursorPosition(0, System.Console.CursorTop+1);
        }
    }

    public enum ExitCode
    {
        Success = 0,
        InvalidInputFile,
    }
}
