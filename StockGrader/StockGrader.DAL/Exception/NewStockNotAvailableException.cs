namespace StockGrader.DAL.Exception
{
    public class NewStocksNotAvailableException : System.Exception
    {
        public NewStocksNotAvailableException(Uri address)
            : base($"New stock reports are not available on address: {address}")
        { }

        public NewStocksNotAvailableException(System.Exception ex)
            : base($"New stock is not available because: {ex.Message}")
        { }
    }
}
