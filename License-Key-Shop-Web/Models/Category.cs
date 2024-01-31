using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryIcon { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
