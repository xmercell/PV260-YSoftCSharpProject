using StockGrader.BL.Model;

namespace StockGrader.BL.Writer
{
    public interface IWriter
    {
        void WriteStockComparison(Diff diff);

        void WriteError(string message);
    }
}
