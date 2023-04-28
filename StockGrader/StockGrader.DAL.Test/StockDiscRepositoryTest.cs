using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockGrader.DAL.Repository;

namespace StockGrader.DAL.Test
{
    public class StockDiscRepositoryTest
    {
        [Test]
        public async Task GetLastTest()
        {
            var filePath = Path.GetTempFileName();
            var url = new Uri("https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv");
            var userAgentHeader = "User-Agent";
            var commonUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
            var stockRep = new StockDiscRepository(url, filePath, userAgentHeader, commonUserAgent);

            try
            {
                await stockRep.FetchNew();
                var lines = await File.ReadAllLinesAsync(filePath);
                var stockReport = stockRep.GetLast();

                Assert.That((lines.Length - 2), Is.EqualTo(stockReport.Entries.Count()));
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Test]
        public async Task FetchTest()
        {
            var filePath = Path.GetTempFileName();
            var url = new Uri("https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv");
            var userAgentHeader = "User-Agent";
            var commonUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
            var stockRep = new StockDiscRepository(url, filePath, userAgentHeader, commonUserAgent);

            try
            {
                await stockRep.FetchNew();
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
