using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockGrader.BL.Model
{
    public class RemovedPosition: AbstractPosition
    {
        public string CompanyName { get; }

        public string Ticker { get; }

        public RemovedPosition(string companyName, string ticker)
        {
            CompanyName = companyName;
            Ticker = ticker;
        }

        public override string ToString()
        {
            return $"{CompanyName}, {Ticker}";
        }
    }
}
