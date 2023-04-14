namespace StockGrader.BL.Writer
{
    public interface IWriter
    {
        void Write(IDiffProvider diffProvider);
    }
}
