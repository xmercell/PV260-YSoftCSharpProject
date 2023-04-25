namespace StockGrader.BL.Model
{
    public class Diff
    {
        public IList<Position> NewPositions { get; init; } = new List<Position>();
        public IList<Position> UnchangedPositions { get; init; } = new List<Position>();
        public IList<UpdatedPosition> IncreasedPositions { get; init; } = new List<UpdatedPosition>();
        public IList<UpdatedPosition> ReducedPositions { get; init; } = new List<UpdatedPosition>();
        public IList<RemovedPosition> RemovedPositions { get; init; } = new List<RemovedPosition>();
    }
}
