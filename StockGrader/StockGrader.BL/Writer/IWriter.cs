using StockGrader.BL.Model;

namespace StockGrader.BL.Writer
{
    public interface IWriter
    {
        void Write(Diff diff);
    }
}
