using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockGrader.BL.Model
{
    public class UpdatedPosition
    {
        public string CompanyName { get; }

        public string Ticker { get; }

        public int Shares { get; }

        public double Weight { get; }

        public double SharesChange { get; }

        public UpdatedPosition(string companyName, string ticker, int shares, double weight, double sharesChange)
        {
            CompanyName = companyName;
            Ticker = ticker;
            Shares = shares;
            Weight = weight;
            SharesChange = sharesChange;
        }

        public override string ToString()
        {
            var changeSymbol = SharesChange < 0 ? "🔻"  : "🔺";
            return $"{CompanyName}, {Ticker}, {Shares}( {changeSymbol} {Math.Abs(SharesChange)}%), {Weight}(%)";
        }
    }
}
