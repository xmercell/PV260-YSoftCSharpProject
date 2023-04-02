using CsvHelper.Configuration;
using System.Globalization;

namespace StockGrader.Domain.Model
{
    public class ReportEntry
    {
        public DateTime Date { get; set; } = DateTime.MinValue;

        public string Fund { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string Ticker { get; set; } = string.Empty;

        public string Cusip { get; set; } = string.Empty;

        public int Shares { get; set; }

        public decimal MarketValue { get; set; }

        public double Weight { get; set; }
    }

    public class ReportEntryMap : ClassMap<ReportEntry>
    {
        public ReportEntryMap()
        {
            Map(m => m.Date).Name("date");
            Map(m => m.Fund).Name("fund");
            Map(m => m.CompanyName).Name("company");
            Map(m => m.Ticker).Name("ticker");
            Map(m => m.Cusip).Name("cusip").Optional();
            Map(m => m.Shares).Convert(row =>
            {
                var s = row.Row.GetField("shares");
                
                if (s == null) return 0;
                return int.Parse(s, NumberStyles.AllowThousands);
            });
            Map(m => m.MarketValue).Convert(row =>
            {
                var m = row.Row.GetField("market value ($)");

                if (m == null) return 0;
                return decimal.Parse(m, NumberStyles.Currency);
            });
            Map(m => m.Weight).Convert(row => {
                var w = row.Row.GetField("weight (%)")?.Replace("%","");

                if (w == null) return 0;
                return double.Parse(w);
            });
        }
    }
}