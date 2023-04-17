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
            var stockRep = new StockDiscRepository(new FileRepository(), new Uri("https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv"), filePath);

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
    }
}
