namespace StockGrader.DAL.Exception
{
    public class LastStockNotFoundException : System.Exception
    {
        public LastStockNotFoundException(string path)
            : base($"Last stored stock could not be found on path: {path}")
        {}

        public LastStockNotFoundException(System.Exception ex)
            : base($"Last stock is not available because of: {ex.Message}")
        { }
    }
}
