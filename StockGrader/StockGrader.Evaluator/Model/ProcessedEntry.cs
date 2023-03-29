using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockGrader.Evaluator.Model
{

    internal class ProcessedEntry
    {
        public string CompanyName { get; }

        public string Ticker { get; }

        public int Shares { get; }

        public double Weight { get; }

        public ProcessedEntry(string companyName, string ticker, int shares, double weight)
        {
            CompanyName = companyName;
            Ticker = ticker;
            Shares = shares;
            Weight = weight;
        }
    }
}
