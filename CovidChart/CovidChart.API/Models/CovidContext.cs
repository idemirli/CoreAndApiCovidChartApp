using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidChart.API.Models
{
    public class CovidContext:DbContext
    {
        public CovidContext(DbContextOptions<CovidContext> options):base(options)
        {

        }
        public DbSet<Covid> Covids { get; set; }
    }
}
