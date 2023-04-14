namespace StockGrader.BL.Exception
{
    public class DuplicateRecordException : System.Exception
    {
        public DuplicateRecordException()
        {
        }

        public DuplicateRecordException(string message)
            : base(message)
        {
        }

        public DuplicateRecordException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
