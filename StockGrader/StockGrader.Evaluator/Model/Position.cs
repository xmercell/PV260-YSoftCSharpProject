using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockGrader.Evaluator.Model
{
    public class Position
    {
        public string CompanyName { get; }

        public string Ticker { get; }

        public int Shares { get; }

        public double Weight { get; }

        public Position(string companyName, string ticker, int shares, double weight)
        {
            CompanyName = companyName;
            Ticker = ticker;
            Shares = shares;
            Weight = weight;
        }

        public override string ToString()
        {
            return $"{CompanyName}, {Ticker}, {Shares}, {Weight}";
        }
    }
}
