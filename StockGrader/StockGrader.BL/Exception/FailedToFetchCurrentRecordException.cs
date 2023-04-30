using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockGrader.BL.Exception
{
    public class FailedToFetchCurrentRecordException : System.Exception
    {
        public FailedToFetchCurrentRecordException()
        {
        }

        public FailedToFetchCurrentRecordException(string message)
            : base(message)
        {
        }

        public FailedToFetchCurrentRecordException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
