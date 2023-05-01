using StockGrader.BL.Exception;
using StockGrader.BL.Model;
using StockGrader.DAL.Exception;
using StockGrader.DAL.Model;
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

        public Diff GetDiffSince(DateTime previousDate)
        {
            StockReport previousStock; 
            try 
            { 
                previousStock = _stockRepository.GetByDate(previousDate);
            }
            catch (LastStockNotFoundException ex)
            {
                throw new FailedToGetOldRecordException("Could not get old record from database.", ex);
            }

            StockReport todayStock;
            try
            {
                todayStock = _stockRepository.GetCurrent();
            }
            catch(NewStocksNotAvailableException ex)
            {
                throw new FailedToFetchCurrentRecordException("Could not get current report from database or fetch new from web.", ex);
            }


            if (previousStock is null)
            {
                previousStock = new StockReport();
            }

            return _diffProvider.CalculateDiff(previousStock.Entries, todayStock.Entries);
        }
    }
}
