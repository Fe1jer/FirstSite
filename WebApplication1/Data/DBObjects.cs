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



            if (!context.Product.Any())
                context.Product.AddRange(
                     new Product
                     {
                         Name = "Galaxy S20",
                         ShortDesc = "Самартфон",
                         LongDesc = "Крутой и догой",
                         img = "/img/samsung-galaxy-s20.png",
                         Price = 1200,
                         IsFavourite = true,
                         Available = true,
                         Category = "Телефон",
                         Company = "Samsung",
                         Country = "Северная Корея"
                     },
                    new Product
                    {
                        Name = "Airpods",
                        ShortDesc = "Наушники класс",
                        LongDesc = "Удобные крутые дорогие",
                        img = "/img/apple_airpods.png",
                        Price = 300,
                        IsFavourite = true,
                        Available = true,
                        Category = "Наушники",
                        Company = "Apple",
                        Country = "США"

                    },
                    new Product
                    {
                        Name = "MacBook Air 13",
                        ShortDesc = "Быстрый и тд",
                        LongDesc = "Хорошая производительность, большой заряд",
                        img = "/img/apple-macbook-air-13.png",
                        Price = 2000,
                        IsFavourite = false,
                        Available = true,
                        Category = "Ноутбук",
                        Company = "Apple",
                        Country = "США"
                    },
                    new Product
                    {
                        Name = "UE43RU7200U 43",
                        ShortDesc = "Большой и приятный",
                        LongDesc = "Крутой экран, звук долби атмос, ну для фильмица найс",
                        img = "/img/Samsung_ue43ru7200u.png",
                        Price = 45000,
                        IsFavourite = true,
                        Available = false,
                        Category = "Телевизор",
                        Company = "Samsung",
                        Country = "Северная Корея"
                    });
            context.SaveChanges();
        }

    }
}