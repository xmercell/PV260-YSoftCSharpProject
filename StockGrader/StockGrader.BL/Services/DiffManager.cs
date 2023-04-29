using StockGrader.BL.Model;
using StockGrader.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockGrader.BL.Services
{
    internal class DiffManager : IDiffManager
    {
        private readonly IStockRepository _stockRepository;
        private readonly IDiffProvider _diffProvider;

        public DiffManager(IStockRepository stockRepository, IDiffProvider diffProvider)
        {
            _stockRepository = stockRepository;
            _diffProvider = diffProvider;
        }

        public Diff GetDailyDiff()
        {
            var today = DateTime.Now;
            var yesterday = today.AddDays(-1);

            var yesterdayStock = _stockRepository.GetByDate(yesterday);


            var todayStock = _stockRepository.GetByDate(today);

            if (todayStock != null) { }


            return _diffProvider.CalculateDiff(yesterdayStock.Entries, todayStock.Entries);
        }

        public Diff GetBiweeklyDiff()
        {
            throw new NotImplementedException();
        }

        public Diff GetMotnhlyDiff()
        {
            throw new NotImplementedException();
        }

        public Diff GetWeeklyDiff()
        {
            throw new NotImplementedException();
        }
    }
}
