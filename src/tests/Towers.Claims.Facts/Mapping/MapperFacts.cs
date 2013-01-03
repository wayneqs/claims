using System;
using Towers.Claims.Mapping;
using Xunit;

namespace Towers.Claims.Facts.Mapping
{
    public class MapperFacts
    {
        [Fact]
        public void ShouldMapColumnsToExpectedTypeBasedOnDefaultFieldOrder()
        {
            var mapper = new Mapper<DefaultOrderTargetType>(new ErrorCollector());
            var target = mapper.Map(new[] {"hello", "4", "23-Jun-2012"});
            Assert.Equal("hello", target.AString);
            Assert.Equal(4, target.AnInt);

            // ok; mapping datetimes like this is a bit dodgy because ParseExact isn't being used under the hood
            // but for the purposes of the narrow scope of this example project i hope its ok.
            Assert.Equal(new DateTime(2012, 6, 23), target.ADateTime);
        }

        [Fact]
        public void ShouldMapColumnsToExpectedTypeBasedOnSpecifiedFieldOrder()
        {
            var mapper = new Mapper<SpecifiedOrderTargetType>(new ErrorCollector());
            var target = mapper.Map(new[] {"4", "hello"});
            Assert.Equal("hello", target.AString);
            Assert.Equal(4, target.AnInt);
        }

        [Fact]
        public void ShouldRecordErrorIfNotEnoughColumnsToExtractFrom()
        {
            var collector = new ErrorCollector();
            Assert.False(collector.ContainsErrors);

            var mapper = new Mapper<DefaultOrderTargetType>(collector);
             mapper.Map(new[] {"hello", "4"});
            Assert.True(collector.ContainsErrors);
            Console.WriteLine(collector.ToString()); // if you're interested
        }

        [Fact]
        public void ShouldRecordErrorIfUnableToParseTheInput()
        {
            var collector = new ErrorCollector();
            Assert.False(collector.ContainsErrors);

            var mapper = new Mapper<SpecifiedOrderTargetType>(collector);
             mapper.Map(new[] {"4,787a", "hello" });
            Assert.True(collector.ContainsErrors);
            Console.WriteLine(collector.ToString()); // if you're interested
        }
    }

    public class DefaultOrderTargetType
    {
        public string AString;
        public int AnInt;
        public DateTime ADateTime;
    }

    public class SpecifiedOrderTargetType
    {
        [FieldIndex(1)]
        public string AString;

        [FieldIndex(0)]
        public int AnInt;
    }
}
