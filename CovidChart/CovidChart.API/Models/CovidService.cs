using CovidChart.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidChart.API.Models
{
    public class CovidService
    {
        private readonly CovidContext _context;
        private readonly IHubContext<CovidHub> _hubContext;

        public CovidService(CovidContext context,IHubContext<CovidHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IQueryable<Covid> GetList()
        {
            return _context.Covids.AsQueryable();
        }

        public async Task SaveCovid(Covid covid)
        {
            await _context.Covids.AddAsync(covid);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList", GetCovidChartList());
        }

        //Pivod Chart Kullanıldı.
        public List<CovidChart> GetCovidChartList()
        {
            List<CovidChart> covidCharts = new List<CovidChart>();
            using (var command=_context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select tarih,[1],[2],[3],[4],[5] FROM (select[City],[Count], CAST([CovidDate] as date) as tarih FROM Covids) as coviDt Pivot (Sum(Count) For City IN([1],[2],[3],[4],[5])) AS ptable order by tarih asc";
                command.CommandType = System.Data.CommandType.Text;
                _context.Database.OpenConnection();
                using (var reader=command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidChart cc = new CovidChart();
                        cc.CovidDate = reader.GetDateTime(0).ToShortDateString();
                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {
                            if (System.DBNull.Value.Equals(reader[x]))
                            {
                                cc.counts.Add(0);
                            }
                            else
                            {
                                cc.counts.Add(reader.GetInt32(x));
                            }
                        });

                        covidCharts.Add(cc);
                    }
                }
                _context.Database.CloseConnection();
                return covidCharts;
            }
        }
    }
}
