using StockGrader.BL.Model;
using StockGrader.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockGrader.BL.Services
{
    public class DiffManager : IDiffManager
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
            var yesterday = DateTime.Now.AddDays(-1);
            return GetDiffSince(yesterday);
        }

        public Diff GetBiweeklyDiff()
        {
            var twoWeeksAgo = DateTime.Now.AddDays(-14);
            return GetDiffSince(twoWeeksAgo);
        }

        public Diff GetMotnhlyDiff()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            return GetDiffSince(lastMonth);
        }

        public Diff GetWeeklyDiff()
        {
            var lastWeek = DateTime.Now.AddDays(-7);
            return GetDiffSince(lastWeek);
        }

        private Diff GetDiffSince(DateTime previousDate)
        {
            var previousStock = _stockRepository.GetByDate(previousDate);
            var todayStock = _stockRepository.GetCurrent();

            if (previousStock is null)
            {
                previousStock = new DAL.Model.StockReport();
            }

            return _diffProvider.CalculateDiff(previousStock.Entries, todayStock.Entries);
        }
    }
}
