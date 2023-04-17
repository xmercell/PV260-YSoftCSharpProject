using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;
using FakeItEasy;
using Moq;
using StockGrader.BL;
using StockGrader.BL.Model;
using StockGrader.BL.Writer;
using StockGrader.DAL.Repository;

namespace StockGrader.Writer.Test
{
    public class WriterUnitTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void IncreseadPositionTest()
        {   
            var diffProvider = A.Fake<IDiffProvider>();
            A.CallTo(() => diffProvider.IncreasedPositions).Returns(new List<UpdatedPosition> { new UpdatedPosition("TESLA INC", "TSLA", 3986021, 10, 2.5) });

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var consoleWriter = new ConsoleWriter();
            consoleWriter.Write(diffProvider);

            var expectedOutput =  $"New positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Increased positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔺 x%), Weight(%){Environment.NewLine}" +
                                  $"TESLA INC, TSLA, 3986021( 🔺 2.5%), 10(%){Environment.NewLine}" +
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
            var diffProvider = A.Fake<IDiffProvider>();
            A.CallTo(() => diffProvider.RemovedPositions).Returns(new List<RemovedPosition> { new RemovedPosition("TESLA INC", "TSLA") });

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var consoleWriter = new ConsoleWriter();
            consoleWriter.Write(diffProvider);

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
            var diffProvider = A.Fake<IDiffProvider>();
            A.CallTo(() => diffProvider.IncreasedPositions).Returns(new List<UpdatedPosition> { new UpdatedPosition("TESLA INC", "TSLA", 3986021, 10, -2.5) });

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var consoleWriter = new ConsoleWriter();
            consoleWriter.Write(diffProvider);

            var expectedOutput = $"New positions:{Environment.NewLine}" +$"Company Name, Ticker, #Shares, Weight(%){Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Increased positions:{Environment.NewLine}" + $"Company Name, Ticker, #Shares( 🔺 x%), Weight(%){Environment.NewLine}" +
                                  $"TESLA INC, TSLA, 3986021( 🔻 2.5%), 10(%){Environment.NewLine}" +
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
            var diffProvider = A.Fake<IDiffProvider>();
            A.CallTo(() => diffProvider.NewPositions).Returns(new List<Position> { new Position("TESLA INC", "TSLA", 3986021, 10) });

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var consoleWriter = new ConsoleWriter();
            consoleWriter.Write(diffProvider);

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
            var diffProvider = A.Fake<IDiffProvider>();
            A.CallTo(() => diffProvider.UnchangedPositions).Returns(new List<Position> { new Position("TESLA INC", "TSLA", 3986021, 10) });

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var consoleWriter = new ConsoleWriter();
            consoleWriter.Write(diffProvider);

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
