using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StockGrader.Domain.Model
{
    public class ReportEntry
    {
        [Required]
        public DateTime Date { get; set; }

        public string Fund { get; set; } = string.Empty;

        [Required]
        [MaxLength(64)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [MaxLength(5)]
        public string Ticker { get; set; } = string.Empty;

        public string Cusip { get; set; } = string.Empty;

        [Required]
        public int Shares { get; set; }

        [Required]
        public double MarketValue { get; set; }

        [Required]
        public double Weight { get; set; }

        public ReportEntry(string line)
        {
            string[] values = line.Split(',');

            Date = Convert.ToDateTime(Regex.Replace(values[0], "[^0-9]", ""));
            Fund = Convert.ToString(values[1]);
            CompanyName = Convert.ToString(values[2]);
            Ticker = Convert.ToString(values[3]);
            Cusip = Convert.ToString(values[4]);
            Shares = Convert.ToInt32(Regex.Replace(values[5], "[^0-9]", ""));
            MarketValue = Convert.ToDouble(Regex.Replace(values[6], "[^0-9.]", ""));
            Weight = Convert.ToDouble(Regex.Replace(values[7], "[^0-9.]", ""));
        }
    }
}