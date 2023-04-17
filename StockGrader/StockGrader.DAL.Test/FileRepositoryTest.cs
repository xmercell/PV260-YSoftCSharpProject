using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using StockGrader.DAL.Repository;

namespace StockGrader.DAL.Test
{
    public class FileRepositoryTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task FetchTest()
        {
            var fileRep = new FileRepository();
            var filePath = Path.GetTempFileName();

            try
            {
                await fileRep.Fetch(new Uri("https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv"), filePath);
                using StreamReader sr = new(File.OpenRead(filePath));
                var header = await sr.ReadLineAsync();
                Assert.That(header, Is.Not.Null);
                Assert.That(header.Equals("date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\""));
            }
            finally
            {
                File.Delete(filePath);
            }
        }
    }
}
