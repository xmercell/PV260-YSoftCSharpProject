using StockGrader.BL.Model;
using StockGrader.BL.Writer;

namespace StockGrader.BL.Test.ConsoleWriterTest
{
    public class When
    {
        private ConsoleWriter _consoleWriter;
        private StringWriter _stringWriter;
        private string currentWriterResult;
        private string currentExpectedOutput;

        public When()
        {
            _consoleWriter = new ConsoleWriter();
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
        }

        public void AssertWriterResultCorrect()
        {
            Assert.That(currentWriterResult, Is.EqualTo(currentExpectedOutput));
        }

        public When IncreasedPositionsBasicWriter()
        {
            var increasedPositions = new List<UpdatedPosition>
            {
                new UpdatedPosition("Company A", "A", 120, 20, 5),
                new UpdatedPosition("Company B", "B", 140, 25, 10)
            };

            currentWriterResult = _consoleWriter.IncreasedPositions(increasedPositions);
            currentExpectedOutput = "Increased positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares( 🔺 x%), Weight(%){Environment.NewLine}" +
                                 "Company A, A, 120( 🔺 5%), 20(%)" + Environment.NewLine +
                                 "Company B, B, 140( 🔺 10%), 25(%)" + Environment.NewLine + Environment.NewLine;
            return this;
        }

        public When NewPositionsBasicWriter()
        {
            var newPositions = new List<Position>
            {
                new Position("Company C", "C", 80, 15),
                new Position("Company D", "D", 60, 10)
            };

            currentWriterResult = _consoleWriter.NewPositions(newPositions);
            currentExpectedOutput = "New positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                 "Company C, C, 80, 15(%)" + Environment.NewLine +
                                 "Company D, D, 60, 10(%)" + Environment.NewLine + Environment.NewLine;
            return this;
        }

        public When ReducedPositionsBasicWriter()
        {
            var reducedPositions = new List<UpdatedPosition>
            {
                new UpdatedPosition("Company E", "E", 100, 22, -8),
                new UpdatedPosition("Company F", "F", 90, 20, -5)
            };

            currentWriterResult = _consoleWriter.ReducedPositions(reducedPositions);
            currentExpectedOutput = "Reduced positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares( 🔻 x%), Weight(%){Environment.NewLine}" +
                                 "Company E, E, 100( 🔻 8%), 22(%)" + Environment.NewLine +
                                 "Company F, F, 90( 🔻 5%), 20(%)" + Environment.NewLine + Environment.NewLine;
            return this;
        }

        public When RemovedPositionsBasicWriter()
        {
            var removedPositions = new List<RemovedPosition>
            {
                new RemovedPosition("Company G", "G"),
                new RemovedPosition("Company H", "H")
            };

            currentWriterResult = _consoleWriter.RemovedPositions(removedPositions);
            currentExpectedOutput = "Removed positions:" + Environment.NewLine + $"Company Name, Ticker{Environment.NewLine}" +
                                 "Company G, G" + Environment.NewLine +
                                 "Company H, H" + Environment.NewLine + Environment.NewLine;
            return this;
        }

        public When UnchangedPositionsBasicWriter()
        {
            var unchangedPositions = new List<Position>
            {
                new Position("Company I", "I", 150, 30),
                new Position("Company J", "J", 130, 26)
            };

            currentWriterResult = _consoleWriter.UnchangedPositions(unchangedPositions);
            currentExpectedOutput = "Unchanged positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                 "Company I, I, 150, 30(%)" + Environment.NewLine +
                                 "Company J, J, 130, 26(%)" + Environment.NewLine + Environment.NewLine;
            return this;
        }

        public When IncreasedPositionWriter()
        {
            var diff = new Diff()
            {
                IncreasedPositions = new List<UpdatedPosition>
                { new UpdatedPosition("TESLA INC", "TSLA", 3986021, 10, 2.5) }
            };

            _consoleWriter.WriteStockComparison(diff);

            currentWriterResult = _stringWriter.ToString();
            currentExpectedOutput = $"New positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Increased positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔺 x%), Weight(%){Environment.NewLine}" +
                                  $"TESLA INC, TSLA, 3986021( 🔺 {2.5}%), 10(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Reduced positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔻 x%), Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Unchanged positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Removed positions:{Environment.NewLine}" + $"Company Name, Ticker{Environment.NewLine}" +
                                  $"{Environment.NewLine}";
            return this;
        }

        public When RemovedPositionWriter()
        {
            var diff = new Diff()
            {
                RemovedPositions = new List<RemovedPosition>
                { new RemovedPosition("TESLA INC", "TSLA") }
            };

            _consoleWriter.WriteStockComparison(diff);

            currentWriterResult = _stringWriter.ToString();
            currentExpectedOutput = $"New positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Increased positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔺 x%), Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Reduced positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔻 x%), Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Unchanged positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Removed positions:{Environment.NewLine}" + $"Company Name, Ticker{Environment.NewLine}" +
                                  $"TESLA INC, TSLA{Environment.NewLine}" +
                                  $"{Environment.NewLine}";
            return this;
        }

        public When ReducedPositionWriter()
        {
            var diff = new Diff()
            {
                IncreasedPositions = new List<UpdatedPosition>
                { new UpdatedPosition("TESLA INC", "TSLA", 3986021, 10, -2.5) }
            };

            _consoleWriter.WriteStockComparison(diff);

            currentWriterResult = _stringWriter.ToString();
            currentExpectedOutput = $"New positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Increased positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔺 x%), Weight(%){Environment.NewLine}" +
                                  $"TESLA INC, TSLA, 3986021( 🔻 {2.5}%), 10(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Reduced positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔻 x%), Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Unchanged positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Removed positions:{Environment.NewLine}" + $"Company Name, Ticker{Environment.NewLine}" +
                                  $"{Environment.NewLine}";
            return this;
        }

        public When NewPositionWriter()
        {
            var diff = new Diff()
            {
                NewPositions = new List<Position>
                { new Position("TESLA INC", "TSLA", 3986021, 10) }
            };

            _consoleWriter.WriteStockComparison(diff);

            currentWriterResult = _stringWriter.ToString();
            currentExpectedOutput = $"New positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"TESLA INC, TSLA, 3986021, 10(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Increased positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔺 x%), Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Reduced positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔻 x%), Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Unchanged positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Removed positions:{Environment.NewLine}" + $"Company Name, Ticker{Environment.NewLine}" +
                                  $"{Environment.NewLine}";
            return this;
        }

        public When UnchangedPositionWriter()
        {
            var diff = new Diff()
            {
                UnchangedPositions = new List<Position>
                { new Position("TESLA INC", "TSLA", 3986021, 10) }
            };

            _consoleWriter.WriteStockComparison(diff);

            currentWriterResult = _stringWriter.ToString();
            currentExpectedOutput = $"New positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Increased positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔺 x%), Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Reduced positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔻 x%), Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Unchanged positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"TESLA INC, TSLA, 3986021, 10(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Removed positions:{Environment.NewLine}" + $"Company Name, Ticker{Environment.NewLine}" +
                                  $"{Environment.NewLine}";
            return this;
        }

        public When DiffWriter()
        {
            var diff = new Diff()
            {
                NewPositions = new List<Position>
               {
                    new Position("Company C", "C", 80, 15),
                    new Position("Company D", "D", 60, 10)
               },
                IncreasedPositions = new List<UpdatedPosition>
               {
                    new UpdatedPosition("Company A", "A", 120, 20, 5),
                    new UpdatedPosition("Company B", "B", 140, 25, 10)
               },
                ReducedPositions = new List<UpdatedPosition>
               {
                    new UpdatedPosition("Company E", "E", 100, 22, -8),
                    new UpdatedPosition("Company F", "F", 90, 20, -5)
               },
                UnchangedPositions = new List<Position>
               {
                    new Position("Company I", "I", 150, 30),
                    new Position("Company J", "J", 130, 26)
               },
                RemovedPositions = new List<RemovedPosition>
               {
                    new RemovedPosition("Company G", "G"),
                    new RemovedPosition("Company H", "H")
               }
            };

            _consoleWriter.WriteStockComparison(diff);
            currentWriterResult = _stringWriter.ToString();
            currentExpectedOutput = "New positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                 "Company C, C, 80, 15(%)" + Environment.NewLine +
                                 "Company D, D, 60, 10(%)" + Environment.NewLine + Environment.NewLine +
                                 "Increased positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares( 🔺 x%), Weight(%){Environment.NewLine}" +
                                 "Company A, A, 120( 🔺 5%), 20(%)" + Environment.NewLine +
                                 "Company B, B, 140( 🔺 10%), 25(%)" + Environment.NewLine + Environment.NewLine +
                                 "Reduced positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares( 🔻 x%), Weight(%){Environment.NewLine}" +
                                 "Company E, E, 100( 🔻 8%), 22(%)" + Environment.NewLine +
                                 "Company F, F, 90( 🔻 5%), 20(%)" + Environment.NewLine + Environment.NewLine +
                                 "Unchanged positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                 "Company I, I, 150, 30(%)" + Environment.NewLine +
                                 "Company J, J, 130, 26(%)" + Environment.NewLine + Environment.NewLine +
                                 "Removed positions:" + Environment.NewLine + $"Company Name, Ticker{Environment.NewLine}" +
                                 "Company G, G" + Environment.NewLine +
                                 "Company H, H" + Environment.NewLine + Environment.NewLine;
            return this;
        }
    }
}
