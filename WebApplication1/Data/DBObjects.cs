using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using System.Data.Entity;

namespace WebApplication1.Data
{
    public class DBObjects
    {
        public static void Initial(AppDBContext context)
        {
/*            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();*/
/*            context.Category.RemoveRange(context.Category);
            context.SaveChanges();
            context.Car.RemoveRange(context.Car);
            context.SaveChanges();*/

            if (!context.Category.Any())
                context.Category.AddRange(Categories.Select(c => c.Value));

            if (!context.Car.Any())
                context.Car.AddRange(
                     new Car
                     {
                         Name = "Volkswagen Polo",
                         ShortDesc = "Бесшумный и экономный",
                         LongDesc = "Небольшой семейный автомобиль для городской жизни",
                         img = "/img/Volkswagen_polo.jpg",
                         Price = 20000,
                         IsFavourite = true,
                         Available = true,
                         Category = Categories["Классические машины"]
                     },
                    new Car
                    {
                        Name = "BMW M3",
                        ShortDesc = "Дерзкий и стильный",
                        LongDesc = "Удобный автомобиль для городской жизни",
                        img = "/img/bmw-m3.jpg",
                        Price = 60000,
                        IsFavourite = true,
                        Available = true,
                        Category = Categories["Классические машины"]
                    },
                    new Car
                    {
                        Name = "Tesla Model X",
                        ShortDesc = "Современный и большой",
                        LongDesc = "Премиальный автомобиль для городской жизни",
                        img = "/img/tesla_model_x_2020.jpg",
                        Price = 65000,
                        IsFavourite = false,
                        Available = true,
                        Category = Categories["Электромобили"]
                    },
                    new Car
                    {
                        Name = "Lexus rx 350",
                        ShortDesc = "Современный и большой",
                        LongDesc = "Премиальный автомобиль для городской жизни",
                        img = "/img/Lexus.jpg",
                        Price = 45000,
                        IsFavourite = true,
                        Available = false,
                        Category = Categories["Классические машины"]
                    }
                    ) ;
            context.SaveChanges();
        }

        private static Dictionary<string, Category> Category;

        public static Dictionary<string, Category> Categories
        {
            get
            {
                if (Category == null)
                {
                    var list = new Category[]
                    {
                        new Category { CategoryName = "Электромобили", Desc = "Современный вид транспорта" },
                        new Category { CategoryName = "Классические машины", Desc = "Машины с двигателем внутреннего сгорания" }

                    };
                    Category = new Dictionary<string, Category>();
                    foreach (Category el in list)
                    {
                        Category.Add(el.CategoryName, el);
                    }
                }
                return Category;
            }
        }
    }
}