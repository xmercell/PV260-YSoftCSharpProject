using System.ComponentModel.DataAnnotations;

namespace StockGrader.Domain.Model
{
    public class ReportEntry
    {
        [Required]
        public DateTime EntryDate { get; set; }

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
        public float Weight { get; set; }
    }
}