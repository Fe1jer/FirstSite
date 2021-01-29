using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data.Models;

namespace WebApplication1.Data
{
    public class DBObjects
    {
        private const string FuelCategory = "Телевизоры";
        private const string ElectroCategory = "Телефоны";

        public static void Initial(AppDBContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            /*          context.Category.RemoveRange(context.Category);
                        context.SaveChanges();
                        context.Car.RemoveRange(context.Car);
                        context.SaveChanges();*/

            if (!context.Category.Any())
                context.Category.AddRange(Categories.Select(c => c.Value));

            if (!context.Сompany.Any())
                context.Сompany.AddRange(Сompanies.Select(c => c));

            if (!context.Product.Any())
                context.Product.AddRange(
                     new Product
                     {
                         Name = "Volkswagen Polo",
                         ShortDesc = "Бесшумный и экономный",
                         LongDesc = "Небольшой семейный автомобиль для городской жизни",
                         img = "/img/Volkswagen_polo.jpg",
                         Price = 20000,
                         IsFavourite = true,
                         Available = true,
                         Category = Categories[FuelCategory],
                         Сompany = Сompanies[0]
                     },
                    new Product
                    {
                        Name = "BMW M3",
                        ShortDesc = "Дерзкий и стильный",
                        LongDesc = "Удобный автомобиль для городской жизни",
                        img = "/img/bmw-m3.jpg",
                        Price = 60000,
                        IsFavourite = true,
                        Available = true,
                        Category = Categories[FuelCategory],
                        Сompany = Сompanies[0]

                    },
                    new Product
                    {
                        Name = "Tesla Model X",
                        ShortDesc = "Современный и большой",
                        LongDesc = "Премиальный автомобиль для городской жизни",
                        img = "/img/tesla_model_x_2020.jpg",
                        Price = 65000,
                        IsFavourite = false,
                        Available = true,
                        Category = Categories[ElectroCategory],
                        Сompany = Сompanies[0]
                    },
                    new Product
                    {
                        Name = "Lexus rx 350",
                        ShortDesc = "Современный и большой",
                        LongDesc = "Премиальный автомобиль для городской жизни",
                        img = "/img/Lexus.jpg",
                        Price = 45000,
                        IsFavourite = true,
                        Available = false,
                        Category = Categories[FuelCategory],
                        Сompany = Сompanies[0]
                    }
                    );
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
                        new Category { CategoryName = "Телевизоры", Desc = "Современный вид транспорта" },
                        new Category { CategoryName = "Телефоны", Desc = "Машины с двигателем внутреннего сгорания" },
                        new Category { CategoryName = "Наушники", Desc = "Машины с двигателем внутреннего сгорания" },
                        new Category { CategoryName = "Ноутбуки", Desc = "Машины с двигателем внутреннего сгорания" },

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

        private static List<Сompany> Сompany;

        public static List<Сompany> Сompanies
        {
            get
            {
                if (Сompany == null)
                {
                    var list = new List<Сompany>()
                    {
                        new Сompany {СompanyName = "Samsung" , Country="Северная Корея"},
                        new Сompany {СompanyName = "LG" , Country="Северная Корея"},
                        new Сompany {СompanyName = "Apple" , Country="США"}
                    };

                    Сompany = new List<Сompany>();

                    foreach (Сompany el in list)
                    {
                        Сompany.Add(el);
                    }
                }
                return Сompany;
            }
        }
    }
}