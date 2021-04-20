using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data.Models
{
    public class News
    {
        public string Name { get; set; }

        public string Img { get; set; }

        public string Desc { get; set; }

        public DateTime CreateData { get; set; }

    }
}
