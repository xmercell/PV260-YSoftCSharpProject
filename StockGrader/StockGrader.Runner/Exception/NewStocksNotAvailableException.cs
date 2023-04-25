using System;

namespace StockGrader.Runner.Exception
{
    public class NewStocksNotAvailableException : System.Exception
    {
        public NewStocksNotAvailableException(Uri address)
            : base($"New stock reports are not available on address: {address}")
        {}
    }
}
