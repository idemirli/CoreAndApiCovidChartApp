using CovidChart.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidChart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController : ControllerBase
    {
        private readonly CovidService _covidService;
        public CovidsController(CovidService covidService)
        {
            _covidService = covidService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _covidService.SaveCovid(covid);
            IQueryable<Covid> covidList = _covidService.GetList();
            return Ok(covidList);
        }

        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random r = new Random();
            //10 gün için vaka girişi
            Enumerable.Range(1, 10).ToList().ForEach(x =>
             {
                 foreach (City item in Enum.GetValues(typeof(City)))
                 {
                     var newCovid = new Covid { City = item, Count = r.Next(100, 1000), CovidDate = DateTime.Now.AddDays(x) };
                      _covidService.SaveCovid(newCovid).Wait();
                     System.Threading.Thread.Sleep(1000);
                 }
             });
            return Ok("Covid dataları veritabanına kaydedildi");
        }
    }
}
