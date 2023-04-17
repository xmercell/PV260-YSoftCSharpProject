using System;
using System.Collections.Generic;
using System.IO;
using FakeItEasy;
using NUnit.Framework;
using StockGrader.BL.Model;
using StockGrader.BL.Writer;
using StockGrader.DAL.Repository;

namespace StockGrader.BL.Test
{
    public class ConsoleWriterTests
    {
        private ConsoleWriter _consoleWriter;
        private IDiffProvider _fakeDiffProvider;

        [SetUp]
        public void SetUp()
        {
            _consoleWriter = new ConsoleWriter();
            _fakeDiffProvider = A.Fake<IDiffProvider>();
        }

        [Test]
        public void Write_ShouldWriteCorrectOutput()
        {
            // Arrange
            A.CallTo(() => _fakeDiffProvider.NewPositions).Returns(new List<Position>
            {
                new Position("Company C", "C", 80, 15),
                new Position("Company D", "D", 60, 10)
            });

            A.CallTo(() => _fakeDiffProvider.IncreasedPositions).Returns(new List<UpdatedPosition>
            {
                new UpdatedPosition("Company A", "A", 120, 20, 5),
                new UpdatedPosition("Company B", "B", 140, 25, 10)
            });

            A.CallTo(() => _fakeDiffProvider.ReducedPositions).Returns(new List<UpdatedPosition>
            {
                new UpdatedPosition("Company E", "E", 100, 22, -8),
                new UpdatedPosition("Company F", "F", 90, 20, -5)
            });

            A.CallTo(() => _fakeDiffProvider.UnchangedPositions).Returns(new List<Position>
            {
                new Position("Company I", "I", 150, 30),
                new Position("Company J", "J", 130, 26)
            });

            A.CallTo(() => _fakeDiffProvider.RemovedPositions).Returns(new List<RemovedPosition>
            {
                new RemovedPosition("Company G", "G"),
                new RemovedPosition("Company H", "H")
            });

            // Act
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                _consoleWriter.Write(_fakeDiffProvider);
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


    }
}