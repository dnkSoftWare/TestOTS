using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using LinqToDB.DataProvider.SqlServer;

namespace TestOTS.Models
{
    public interface IDataRepository
    {
        List<ZnHead> GetDataFromDB(string dt);
    }

    public class DataRepository : IDataRepository
    {
        private DataContext db = null;

        public DataRepository(string connstr)
        {
            db = new DataContext(connstr);
        }

        public List<ZnHead> GetDataFromDB(string dt)
        {
            var data =
                db.ExecuteQuery<ZnData>(string.Format("SELECT [zn_date] as ZnDate,[fio] as Name, [zn_from] as ZnFrom, [zn_to] as ZnTo, [all_hours] as AllHours, zn_idss as ZnIdss FROM [OTSDB].[dbo].[ZnData] where zn_date = '{0}'",dt)).ToList(); 

            var gr_data = data.GroupBy(d =>
                    new { d.ZnDate, d.Name }) // группировка
                .Select(result =>
                    new ZnHead()
                    {
                        ZnDate = result.Key.ZnDate,
                        Fio = result.Key.Name,
                        AllHours = data.Where(d => (d.ZnDate == result.Key.ZnDate && d.Name == result.Key.Name)).Sum(d=>d.AllHours),
  //                      FromHours = data.Where(d => (d.ZnDate == result.Key.ZnDate && d.Name == result.Key.Name))
  //                          .Select(d=>d.ZnFrom).ToArray(),
                        Zn = data.Where(d => (d.ZnDate == result.Key.ZnDate && d.Name == result.Key.Name))
                            .Select(d =>
                                new ZnDetail()
                                {
                                    ZnFrom = d.ZnFrom,
                                    ZnTo = d.ZnTo,
                                    ZnAllHours = d.AllHours,
                                    ZnIdss = d.ZnIdss
                                }
                            ).ToList()
                    }).ToList();
            return gr_data;
        }

    }
}