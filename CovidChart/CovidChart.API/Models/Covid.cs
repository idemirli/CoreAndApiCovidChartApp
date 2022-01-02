using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidChart.API.Models
{
    
    public class Covid
    {
        public int Id { get; set; }

        public City City { get; set; }

        public int Count { get; set; }

        public DateTime CovidDate { get; set; }
    }
}
