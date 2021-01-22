using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;

namespace WebApplication1.Data.Mocks
{
    public class MockCars : IAllCars
    {
        private readonly ICarsCategory _categoryCars = new MockCategory();

        public IEnumerable<Car> Cars
        {
            get
            {
                return new List<Car>
                {
                    new Car{
                        Name = "Volkswagen Polo",
                        ShortDesc="Бесшумный и экономный",
                        LongDesc="Небольшой семейный автомобиль для городской жизни",
                        img="/img/Volkswagen_polo.jpg", 
                        Price=20000, 
                        IsFavourite=true,
                        Available=true, 
                        Category = _categoryCars.AllCategories.Last()
                    },
                    new Car{
                        Name = "BMW M3",
                        ShortDesc="Дерзкий и стильный",
                        LongDesc="Удобный автомобиль для городской жизни",
                        img="/img/bmw-m3.jpg", 
                        Price=60000, 
                        IsFavourite=true,
                        Available=true, 
                        Category = _categoryCars.AllCategories.Last()
                    },
                    new Car{
                        Name = "Tesla Model X",
                        ShortDesc="Современный и большой",
                        LongDesc="Премиальный автомобиль для городской жизни",
                        img="/img/tesla_model_x_2020.jpg", 
                        Price=65000, 
                        IsFavourite=false,
                        Available=true, 
                        Category = _categoryCars.AllCategories.First()
                    },
                    new Car{
                        Name = "Lexus rx 350",
                        ShortDesc="Современный и большой",
                        LongDesc="Премиальный автомобиль для городской жизни",
                        img="/img/Lexus.jpg", 
                        Price=45000, 
                        IsFavourite=true,
                        Available=false, 
                        Category = _categoryCars.AllCategories.Last()
                    },
                };
            }
        }

        public IEnumerable<Car> GetFavCars { get; set; }

        public Car GetObjectCar(int carId)
        {
            throw new NotImplementedException();
        }
    }
}