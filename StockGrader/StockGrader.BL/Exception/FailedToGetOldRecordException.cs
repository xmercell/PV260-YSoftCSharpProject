using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockGrader.BL.Exception
{
    public class FailedToGetOldRecordException : System.Exception
    {
        public FailedToGetOldRecordException()
        {
        }

        public FailedToGetOldRecordException(string message)
            : base(message)
        {
        }

        public FailedToGetOldRecordException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
