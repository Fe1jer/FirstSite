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
/*            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();*/
            /*          context.Category.RemoveRange(context.Category);
                        context.SaveChanges();
                        context.Car.RemoveRange(context.Car);
                        context.SaveChanges();*/

            if (!context.Product.Any())
                context.Product.AddRange(
                     new Product
                     {
                         Name = "Galaxy S20",
                         ShortDesc = "Android, экран 6.9AMOLED(1440x3200), Exynos 990, ОЗУ 12 ГБ, флэш - память 128 ГБ, карты памяти, камера 108 Мп, аккумулятор 5000 мАч,2 SIM",
                         LongDesc = "Европейская версия:\n" +
                         "SoC Exynos 990,\n" +
                         "8 ядер(2×Mongoose M5 @2, 73 ГГц + 2×Cortex - A76 @2, 50 ГГц + 4×Cortex - A55 @2, 0 ГГц)\n" +
                         "GPU Mali - G77 MP11\n" +
                         "Версия для США:\n" +
                         "SoC Qualcomm Snapdragon 865\n" +
                         "GPU Adreno 650\n" +
                         "Операционная система Android 10.0; One UI 2\n" +
                         "Сенсорный дисплей Dynamic AMOLED 6,2″, 3200×1440, 20:9, 563 ppi, 120 Гц при разрешении 2400×1080\n" +
                         "Оперативная память(RAM) 8 ГБ, внутренняя память 128 ГБ\n" +
                         "Поддержка microSD(совмещенный разъем)\n" +
                         "Поддержка Nano-SIM(2 шт.)\n" +
                         "Сети HSPA 42,2 / 5,76 Мбит / с, LTE - A(7CA) Cat.20 2000 / 200 Мбит / с\n" +
                         "GPS / A - GPS, Глонасс, BDS, Galileo\n" +
                         "Wi - Fi 802.11a / b / g / n / ac / ax(2, 4 и 5 ГГц)\n" +
                         "Bluetooth 5.0, A2DP, LE NFC\n" +
                         "USB 3.2 Type - C, USB OTG\n" +
                         "3,5 - миллиметрового аудиовыхода нет\n" +
                         "Камера 12 Мп(f / 1, 8) + 64 Мп(f / 2, 0) + 12 Мп(f / 2, 2), 4320p@24 fps, 2160p@60 fps\n" +
                         "Фронтальная камера 10 Мп(f / 2, 2)\n" +
                         "Датчики приближения и освещения, магнитного поля, акселерометр, гироскоп, барометр\n" +
                         "Сканер отпечатков пальцев под экраном(ультразвуковой)\n" +
                         "Защита от воды IP68(30 минут на глубине до 1,5 м)\n" +
                         "Аккумулятор 4000 мА·ч, быстрая зарядка 25 Вт, беспроводная зарядка 15 Вт\n" +
                         "Размеры 152×69×7,9 мм\n" +
                         "Масса 163 г\n",
                         Img = "/img/samsung-galaxy-s20.png",
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
                        ShortDesc = "Наушники AirPods в футляре с возможностью беспроводной зарядки",
                        LongDesc = "Автоматическое включение и подключение к устройству\n" +
                        "Простая настройка для работы с любыми устройствами Apple\n" +
                        "Двойное касание для начала воспроизведения или переключения на следующий трек\n" +
                        "Новый чип H1 от Apple обеспечивает более быстрое беспроводное подключение наушников к устройствам\n" +
                        "Быстрая подзарядка в футляре\n" +
                        "Футляр можно заряжать с помощью устройств для беспроводной зарядки стандарта Qi или через разъём Lightning\n" +
                        "Высокое качество звучания музыки и голоса\n" +
                        "Моментальное переключение с одного устройства на другое\n" +
                        "Целый день телефонных разговоров и прослушивания музыки благодаря футляру,\n" +
                        "который обеспечивает наушникам несколько циклов зарядки\n",
                        Img = "/img/apple_airpods.png",
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
                        ShortDesc = "Чип Apple M1 с 8‑ядерным процессором и 7‑ядерным графическим процессором. Накопитель 256 ГБ",
                        LongDesc = "Чип Apple M1 с 8‑ядерным процессором, 7‑ядерным графическим процессором и 16‑ядерной системой Neural Engine\n" +
                        "8 ГБ объединённой памяти\n" +
                        "SSD‑накопитель 256 ГБ\n" +
                        "Дисплей Retina с технологией True Tone\n" +
                        "Клавиатура Magic Keyboard\n" +
                        "Touch ID\n" +
                        "Трекпад Force Touch\n" +
                        "Два порта Thunderbolt / USB 4\n",
                        Img = "/img/apple-macbook-air-13.png",
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
                        ShortDesc = "43 3840x2160(4K UHD), матрица VA, частота матрицы 50 Гц, индекс динамичных сцен 1400, Smart TV(Samsung Tizen), HDR, Wi - Fi",
                        LongDesc = "разрешение: 4K UHD (3840x2160), HDR\n" +
                        "диагональ экрана: 43, VA\n" +
                        "тип подсветки: Edge LED\n" +
                        "частота обновления экрана: 50 Гц\n" +
                        "формат HDR: HDR10,\n" +
                        "мощность звука: 20 Вт(2х10 Вт)\n" +
                        "беспроводные интерфейсы: Wi - Fi 802.11n, Bluetooth, Miracast\n" +
                        "проводные интерфейсы: HDMI x 3, USB x 2, Ethernet, выход аудио оптический\n" +
                        "размеры без подставки(ШxВxГ): 970x563x58 мм\n" +
                        "размеры с подставкой(ШxВxГ): 970x648x344 мм\n" +
                        "вес: 12.1 кг\n",
                        Img = "/img/Samsung_ue43ru7200u.png",
                        Price = 2000,
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