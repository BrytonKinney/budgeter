using System;
using System.Collections.Generic;
using System.Text;

namespace Budgeter.FileParsers
{
    public class ParseException : Exception
    {
        public ParseException(string message, Exception innerException, int failedAtColumn) : base(message, innerException)
        {
            FailedAtColumnIndex = failedAtColumn;
        }

        public int FailedAtColumnIndex { get; private set; }
    }
}
