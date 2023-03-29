using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockGrader.Evaluator.Model
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
    }
}
