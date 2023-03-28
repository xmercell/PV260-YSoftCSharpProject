using System.ComponentModel.DataAnnotations;

namespace StockGrader.Domain.Model
{
    public class ReportEntry
    {
        [Required]
        public DateTime Date { get; set; }

        public string Fund { get; set; }

        [Required]
        [MaxLength(64)]
        public string CompanyName { get; set; } = null!;

        [Required]
        [MaxLength(5)]
        public string Ticker { get; set; } = null!;

        public string Cusip { get; set; }

        [Required]
        public int Shares { get; set; }

        [Required]
        public double MarketValue { get; set; }

        [Required]
        public double Weight { get; set; }

        public static ReportEntry FromCSV(string line)
        {
            string[] values = line.Split(',');
            ReportEntry entry = new()
            {
                Date = Convert.ToDateTime(values[0]),
                Fund = Convert.ToString(values[1]),
                CompanyName = Convert.ToString(values[2]),
                Ticker = Convert.ToString(values[3]),
                Cusip = Convert.ToString(values[4]),
                Shares = Convert.ToInt32(values[5]),
                MarketValue = Convert.ToDouble(values[6]),
                Weight = Convert.ToDouble(values[7])
            };

            return entry;
        }
    }
}