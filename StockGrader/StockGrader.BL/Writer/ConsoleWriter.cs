using System.Text;
using StockGrader.BL.Model;


namespace StockGrader.BL.Writer
{
    public class ConsoleWriter : IWriter
    {
        public void WriteStockComparison(Diff diff)
        {
            Console.Write(NewPositions(diff.NewPositions));
            Console.Write(IncreasedPositions(diff.IncreasedPositions));
            Console.Write(ReducedPositions(diff.ReducedPositions));
            Console.Write(UnchangedPositions(diff.UnchangedPositions));
            Console.Write(RemovedPositions(diff.RemovedPositions));
        }

        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }

        internal string IncreasedPositions(IEnumerable<UpdatedPosition> IncreasedPositions)
        {
            var result = new StringBuilder(
                $"Increased positions:{Environment.NewLine}" +
                $"Company Name, Ticker, #Shares( 🔺 x%), Weight(%){Environment.NewLine}");
            foreach (var pos in IncreasedPositions)
            {
                result.AppendLine(pos.ToString());
                
            }
            result.AppendLine();
            return result.ToString();   
        }

        internal string NewPositions(IEnumerable<Position> NewPositions)
        {
            var result = new StringBuilder($"New positions:{Environment.NewLine}" +
                $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}");
            foreach (var pos in NewPositions)
            {
                result.AppendLine(pos.ToString());
            }
            result.AppendLine();
            return result.ToString();
        }

        internal string ReducedPositions(IEnumerable<UpdatedPosition> ReducedPositions)
        {
            var result = new StringBuilder($"Reduced positions:{Environment.NewLine}" +
                $"Company Name, Ticker, #Shares( 🔻 x%), Weight(%){Environment.NewLine}");
            foreach (var pos in ReducedPositions)
            {
                result.AppendLine(pos.ToString());
            }
            result.AppendLine();
            return result.ToString();
        }

        internal string RemovedPositions(IEnumerable<RemovedPosition> RemovedPositions)
        {
            var result = new StringBuilder($"Removed positions:{Environment.NewLine}" + 
                $"Company Name, Ticker{Environment.NewLine}");
            foreach (var pos in RemovedPositions)
            {
                result.AppendLine(pos.ToString());
            }
            result.AppendLine();
            return result.ToString();
        }

        internal string UnchangedPositions(IEnumerable<Position> UnchangedPositions)
        {
            var result = new StringBuilder($"Unchanged positions:{Environment.NewLine}" +
                $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}");
            foreach (var pos in UnchangedPositions)
            {
                result.AppendLine(pos.ToString());
            }
            result.AppendLine();
            return result.ToString();
        }
    }
}
