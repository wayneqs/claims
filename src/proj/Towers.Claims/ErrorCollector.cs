using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Towers.Claims
{
    public class ErrorCollector
    {
        private readonly List<string> _errors = new List<string>();

        public ErrorCollector()
        {
            Context = new List<string>();
        }
        public bool ContainsErrors
        {
            get { return _errors.Count > 0; }
        }

        public List<string> Context { get; private set; }

        public void Add(string error, params object[] args)
        {
            _errors.Add(string.Format(error, args));
        }

        public override string ToString()
        {
            var context = Context.Any() ? Context.Aggregate((a, b) => a + b) : string.Empty;
            var builder = new StringBuilder();
            foreach (var error in _errors)
            {
                if( !string.IsNullOrWhiteSpace(context) ) builder.Append(context);
                builder.AppendLine(error);
            }
            return builder.ToString();
        }

        public void WriteTo(string path)
        {
            using( var file = new StreamWriter(path) )
            {
                file.Write(ToString());
            }
        }

        public void Clear()
        {
            _errors.Clear();
        }

        public TentativeErrorCollector TentativelyAdd(string message, params object[] args)
        {
            return new TentativeErrorCollector(this, string.Format(message, args));
        }

        // temporary collector
        public class TentativeErrorCollector : ErrorCollector, IDisposable
        {
            private readonly ErrorCollector _underlyingCollector;
            private readonly string _contextMessage;

            public TentativeErrorCollector(ErrorCollector underlyingCollector, string contextMessage)
            {
                _underlyingCollector = underlyingCollector;
                _contextMessage = contextMessage;
            }

            public void Commit()
            {
                _underlyingCollector.Add(_contextMessage);
                foreach (var error in _errors)
                {
                    _underlyingCollector.Add(error);
                }
            }

            private bool _rolledBack;
            public void Rollback()
            {
                _rolledBack = true;
            }

            public void Dispose()
            {
                if( !_rolledBack) Commit();
            }
        }
    }
}