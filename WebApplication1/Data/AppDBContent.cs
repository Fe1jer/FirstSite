using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;

namespace WebApplication1.Data
{
    public class AppDBContent :DbContext
    {
        public AppDBContent(DbContextOptions<AppDBContent> option): base(option)
        {

        }
        
        public DbSet<Car> Car { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}
