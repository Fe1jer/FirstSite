using System.Collections.Generic;

namespace InternetShop.ViewModels
{
    public class FilterCategoryVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<FilterSelectionVM> Selections { get; set; }
    }
    public class FilterSelectionVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
