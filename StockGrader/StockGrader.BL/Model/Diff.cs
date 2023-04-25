namespace StockGrader.BL.Model
{
    public class Diff
    {
        public IList<Position> NewPositions { get; } = new List<Position>();
        public IList<Position> UnchangedPositions { get; } = new List<Position>();
        public IList<UpdatedPosition> IncreasedPositions { get; } = new List<UpdatedPosition>();
        public IList<UpdatedPosition> ReducedPositions { get; } = new List<UpdatedPosition>();
        public IList<RemovedPosition> RemovedPositions { get; } = new List<RemovedPosition>();
    }
}
