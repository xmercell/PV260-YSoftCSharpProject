using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockGrader.Evaluator.Exceptions
{
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException()
        {
        }

        public DuplicateRecordException(string message)
            : base(message)
        {
        }

        public DuplicateRecordException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
