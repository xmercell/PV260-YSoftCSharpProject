using FakeItEasy;
using StockGrader.BL.Model;
using StockGrader.BL.Writer;

namespace StockGrader.BL.Test
{
    public class ConsoleWriterTests
    {
        private ConsoleWriter _consoleWriter;

        [SetUp]
        public void SetUp()
        {
            _consoleWriter = new ConsoleWriter();
        }

        [Test]
        public void Write_ShouldWriteCorrectOutput()
        {
            // Arrange
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

            
            // Act
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                _consoleWriter.WriteStockComparison(diff);
                // Assert
                var expectedOutput = "New positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
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
                Assert.That(stringWriter.ToString(), Is.EqualTo(expectedOutput));
            }
        }


        [Test]
        public void IncreasedPositions_ShouldReturnCorrectString()
        {
            // Arrange
            var increasedPositions = new List<UpdatedPosition>
            {
                new UpdatedPosition("Company A", "A", 120, 20, 5),
                new UpdatedPosition("Company B", "B", 140, 25, 10)
            };

            // Act
            var result = _consoleWriter.IncreasedPositions(increasedPositions);

            // Assert
            var expectedString = "Increased positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares( 🔺 x%), Weight(%){Environment.NewLine}" +
                                 "Company A, A, 120( 🔺 5%), 20(%)" + Environment.NewLine +
                                 "Company B, B, 140( 🔺 10%), 25(%)" + Environment.NewLine + Environment.NewLine;
            Assert.That(result, Is.EqualTo(expectedString));
        }

        [Test]
        public void NewPositions_ShouldReturnCorrectString()
        {
            var newPositions = new List<Position>
            {
                new Position("Company C", "C", 80, 15),
                new Position("Company D", "D", 60, 10)
            };

            // Act
            var result = _consoleWriter.NewPositions(newPositions);

            // Assert
            var expectedString = "New positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                 "Company C, C, 80, 15(%)" + Environment.NewLine +
                                 "Company D, D, 60, 10(%)" + Environment.NewLine + Environment.NewLine;
            Assert.That(result, Is.EqualTo(expectedString));
        }

        [Test]
        public void ReducedPositions_ShouldReturnCorrectString()
        {
            // Arrange
            var reducedPositions = new List<UpdatedPosition>
            {
                new UpdatedPosition("Company E", "E", 100, 22, -8),
                new UpdatedPosition("Company F", "F", 90, 20, -5)
            };

            // Act
            var result = _consoleWriter.ReducedPositions(reducedPositions);

            // Assert
            var expectedString = "Reduced positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares( 🔻 x%), Weight(%){Environment.NewLine}" +
                                 "Company E, E, 100( 🔻 8%), 22(%)" + Environment.NewLine +
                                 "Company F, F, 90( 🔻 5%), 20(%)" + Environment.NewLine + Environment.NewLine;
            Assert.That(result, Is.EqualTo(expectedString));
        }

        [Test]
        public void RemovedPositions_ShouldReturnCorrectString()
        {
            // Arrange
            var removedPositions = new List<RemovedPosition>
            {
                new RemovedPosition("Company G", "G"),
                new RemovedPosition("Company H", "H")
            };

            // Act
            var result = _consoleWriter.RemovedPositions(removedPositions);

            // Assert
            var expectedString = "Removed positions:" + Environment.NewLine + $"Company Name, Ticker{Environment.NewLine}" +
                                 "Company G, G" + Environment.NewLine +
                                 "Company H, H" + Environment.NewLine + Environment.NewLine;
            Assert.That(result, Is.EqualTo(expectedString));
        }

        [Test]
        public void UnchangedPositions_ShouldReturnCorrectString()
        {
            // Arrange
            var unchangedPositions = new List<Position>
            {
                new Position("Company I", "I", 150, 30),
                new Position("Company J", "J", 130, 26)
            };

            // Act
            var result = _consoleWriter.UnchangedPositions(unchangedPositions);

            // Assert
            var expectedString = "Unchanged positions:" + Environment.NewLine + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                 "Company I, I, 150, 30(%)" + Environment.NewLine +
                                 "Company J, J, 130, 26(%)" + Environment.NewLine + Environment.NewLine;
            Assert.That(result, Is.EqualTo(expectedString));
        }

        [Test]
        public void IncreseadPositionTest()
        {
            var diff = new Diff()
            {
                IncreasedPositions = new List<UpdatedPosition>
                { new UpdatedPosition("TESLA INC", "TSLA", 3986021, 10, 2.5) }
            };

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var consoleWriter = new ConsoleWriter();
            consoleWriter.WriteStockComparison(diff);

            var expectedOutput = $"New positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
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

            Assert.That(stringWriter.ToString(), Is.EqualTo(expectedOutput));
        }


        [Test]
        public void RemovedPositionTest()
        {
            var diff = new Diff()
            {
                RemovedPositions = new List<RemovedPosition> 
                { new RemovedPosition("TESLA INC", "TSLA") }
            };

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var consoleWriter = new ConsoleWriter();
            consoleWriter.WriteStockComparison(diff);

            var expectedOutput = $"New positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
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

            Assert.That(stringWriter.ToString(), Is.EqualTo(expectedOutput));

        }


        [Test]
        public void ReducedPositionTest()
        {
            var diff = new Diff()
            {
                IncreasedPositions = new List<UpdatedPosition> 
                { new UpdatedPosition("TESLA INC", "TSLA", 3986021, 10, -2.5) }
            };

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var consoleWriter = new ConsoleWriter();
            consoleWriter.WriteStockComparison(diff);

            var expectedOutput = $"New positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
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

            Assert.That(stringWriter.ToString(), Is.EqualTo(expectedOutput));

        }

        [Test]
        public void NewPositionTest()
        {
            var diff = new Diff()
            {
                NewPositions = new List<Position>
                { new Position("TESLA INC", "TSLA", 3986021, 10) }
            };

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var consoleWriter = new ConsoleWriter();
            consoleWriter.WriteStockComparison(diff);

            var expectedOutput = $"New positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
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

            Assert.That(stringWriter.ToString(), Is.EqualTo(expectedOutput));

        }

        [Test]
        public void UnchangedPositionTest()
        {
            var diff = new Diff()
            {
                UnchangedPositions = new List<Position>
                { new Position("TESLA INC", "TSLA", 3986021, 10) }
            };

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var consoleWriter = new ConsoleWriter();
            consoleWriter.WriteStockComparison(diff);

            var expectedOutput = $"New positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
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

            Assert.That(stringWriter.ToString(), Is.EqualTo(expectedOutput));

        }


    }
}