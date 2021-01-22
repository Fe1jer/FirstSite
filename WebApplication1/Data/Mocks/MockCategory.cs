using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;

namespace WebApplication1.Data.Mocks
{
    public class MockCategory : ICarsCategory
    {
        public IEnumerable<Category> AllCategories
        {
            get 
            {
                return new List<Category> {
                    new Category {CategoryName="Электромобили", Desc="Современный вид транспорта" },
                    new Category {CategoryName="Классические машины", Desc="Машины с двигателем внутреннего сгорания" }
                };
            }
        }
    }
}
