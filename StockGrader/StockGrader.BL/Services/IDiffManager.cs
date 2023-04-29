using StockGrader.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockGrader.BL.Services
{
    public interface IDiffManager
    {
        public Diff GetDailyDiff();

        public Diff GetWeeklyDiff();

        public Diff GetBiweeklyDiff();
        public Diff GetMotnhlyDiff();

    }
}
