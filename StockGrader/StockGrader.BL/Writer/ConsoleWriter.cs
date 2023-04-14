using System.Text;
using StockGrader.BL.Model;

namespace StockGrader.BL.Writer
{
    public class ConsoleWriter : IWriter
    {
        public string IncreasedText { get; set; } = string.Empty;
        public string NewText { get; set; } = string.Empty;
        public string UnchangedText { get; set; } = string.Empty;
        public string RemovedText { get; set; } = string.Empty;
        public string ReducedText { get; set; } = string.Empty;

        public void Write(IDiffProvider diffProvider)
        {
            NewText = NewPositions(diffProvider.NewPositions);
            IncreasedText = IncreasedPositions(diffProvider.IncreasedPositions);
            ReducedText = ReducedPositions(diffProvider.ReducedPositions); 
            UnchangedText = UnchangedPositions(diffProvider.UnchangedPositions);
            RemovedText = RemovedPositions(diffProvider.RemovedPositions);

            Console.WriteLine(NewText);
            Console.WriteLine(IncreasedText);
            Console.WriteLine(ReducedText);
            Console.WriteLine(UnchangedText);
            Console.WriteLine(RemovedText);
        }

        private string IncreasedPositions(IEnumerable<UpdatedPosition> IncreasedPositions)
        {
            var result = new StringBuilder($"Increased positions:{Environment.NewLine}");
            foreach (var pos in IncreasedPositions)
            {
                result.AppendLine(pos.ToString());
                
            }
            result.AppendLine();
            return result.ToString();   
        }

        private string NewPositions(IEnumerable<Position> NewPositions)
        {
            var result = new StringBuilder($"New positions:{Environment.NewLine}");
            foreach (var pos in NewPositions)
            {
                result.AppendLine(pos.ToString());
            }
            result.AppendLine();
            return result.ToString();
        }

        private string ReducedPositions(IEnumerable<UpdatedPosition> ReducedPositions)
        {
            var result = new StringBuilder($"Reduced positions:{Environment.NewLine}");
            foreach (var pos in ReducedPositions)
            {
                result.AppendLine(pos.ToString());
            }
            result.AppendLine();
            return result.ToString();
        }

        private string RemovedPositions(IEnumerable<RemovedPosition> RemovedPositions)
        {
            var result = new StringBuilder($"Removed positions:{Environment.NewLine}");
            foreach (var pos in RemovedPositions)
            {
                result.AppendLine(pos.ToString());
            }
            result.AppendLine();
            return result.ToString();
        }

        private string UnchangedPositions(IEnumerable<Position> UnchangedPositions)
        {
            var result = new StringBuilder($"Unchanged positions:{Environment.NewLine}");
            foreach (var pos in UnchangedPositions)
            {
                result.AppendLine(pos.ToString());
            }
            result.AppendLine();
            return result.ToString();
        }
    }
}
