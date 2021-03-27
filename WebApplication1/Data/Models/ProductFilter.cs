namespace WebApplication1.Data.Models
{
    public class ProductFilter
    {
        public string AllCategory { set; get; }
        public string AllCompany { set; get; }
        public string AllCountry { set; get; }

        public bool IsEmpty()
        {
            return AllCategory == null && AllCompany == null && AllCountry == null;
        }

    }
}
