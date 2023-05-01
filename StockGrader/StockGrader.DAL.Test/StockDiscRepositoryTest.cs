using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockGrader.DAL.Repository;

namespace StockGrader.DAL.Test
{
    public class StockDiscRepositoryTest
    {
        [Test]
        public void StockDiskRepositoryLoadsCsv()
        {
            new When().LoadNewReportAsync().Result.AssertReportNotEmpty();
        }

        [Test]
        public void GetByRealDateTest()
        {
            new When().LoadReportValidDate().AssertReportEntriesCount();
        }

        [Test]
        public void GetByNonExistDateTest()
        {
            new When().LoadReportInvalidDate().AssertReportEmpty();
        }


    }
}
