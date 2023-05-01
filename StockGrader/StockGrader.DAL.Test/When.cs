using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using StockGrader.DAL.Model;
using StockGrader.DAL.Repository;

namespace StockGrader.DAL.Test
{
    public class When
    {
        private string endpointUri;
        private string primaryKey;
        private string databaseName;
        private string containerName;

        private Uri url;
        private string userAgentHeader;
        private string commonUserAgent;

        private StockDiscRepository stockRepository;
        private StockReport currentStockReport;

        public When()
        {
            Setup();
            stockRepository = new StockDiscRepository(url, userAgentHeader, commonUserAgent, endpointUri, primaryKey, databaseName, containerName);
        }

        private void Setup()
        {
            endpointUri = "https://matejgros.documents.azure.com:443/";
            primaryKey = "nmMSVZAF1MHieaJJX7jYYKtDub0ivxLHQJjdWha4wX11LCMX8R5uMIQoKSOPdcQ8Z9H7F7APGwvbACDblhWZPw==";
            databaseName = "ysoft";
            containerName = "ysoft_con";

            url = new Uri("https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv");
            userAgentHeader = "User-Agent";
            commonUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
        }

        public When LoadReportInvalidDate()
        {
            currentStockReport = stockRepository.GetByDate(new DateTime(2023, 3, 28, 1, 1, 1, DateTimeKind.Utc));
            return this;
        }

        public When LoadReportValidDate()
        {
            currentStockReport = stockRepository.GetByDate(new DateTime(2023, 4, 28, 1, 1, 1, DateTimeKind.Utc));
            return this;
        }

        public async Task<When> LoadNewReportAsync()
        {
            await stockRepository.FetchNew();
            currentStockReport = stockRepository.GetLast();
            return this;
        }

        public void AssertReportNotEmpty()
        {
            Assert.That(currentStockReport, Is.Not.Null);
            Assert.That(currentStockReport.Entries, Is.Not.Null);
            Assert.That(currentStockReport.Entries.Count(), Is.GreaterThan(0));
        }

        public void AssertReportEmpty()
        {
            Assert.That(currentStockReport, Is.Null);
        }

        public void AssertReportEntriesCount()
        {
            AssertReportNotEmpty();
            Assert.That(currentStockReport.Entries.Count(), Is.EqualTo(28)); ;
        }
    }
}
