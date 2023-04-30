using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockGrader.DAL.Repository;

namespace StockGrader.DAL.Test
{
    public class StockDiscRepositoryTest
    {


        private StockDiscRepository stockRep;

        [SetUp]
        public void SetUp()
        {
            var configFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\StockGrader.ExecutableConsoleApp\\App.config"));
            var configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            var endpointUri = config.AppSettings.Settings["EndPointUri"].Value;
            var primaryKey = config.AppSettings.Settings["PrimaryKey"].Value;
            var databaseName = config.AppSettings.Settings["DatabaseName"].Value;
            var containerName = config.AppSettings.Settings["ContainerName"].Value;

            var url = new Uri("https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv");
            var userAgentHeader = "User-Agent";
            var commonUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
            stockRep = new StockDiscRepository(url, userAgentHeader, commonUserAgent, endpointUri, primaryKey, databaseName, containerName);
        }

        [Test]
        public async Task StockDiskRepositoryLoadsCsv()
        {
            await stockRep.FetchNew();
            var stockReport = stockRep.GetLast();
            Assert.That(stockReport, Is.Not.Null);
            Assert.That(stockReport.Entries, Is.Not.Null);
            Assert.That(stockReport.Entries.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void GetByRealDateTest()
        {
            var stockReport = stockRep.GetByDate(new DateTime(2023, 4, 28, 1, 1, 1, DateTimeKind.Utc));
            Assert.That(stockReport, Is.Not.Null);
            Assert.That(stockReport.Entries, Is.Not.Null);
            Assert.That(stockReport.Entries.Count(), Is.EqualTo(28));
        }

        [Test]
        public void GetByNonExistDateTest()
        {
            var stockReport = stockRep.GetByDate(new DateTime(2023, 3, 28, 1, 1, 1, DateTimeKind.Utc));
            Assert.That(stockReport, Is.Null);

        }


    }
}
