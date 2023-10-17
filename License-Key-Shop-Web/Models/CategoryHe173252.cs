using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class CategoryHe173252
    {
        public CategoryHe173252()
        {
            ProductHe173252s = new HashSet<ProductHe173252>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<ProductHe173252> ProductHe173252s { get; set; }
    }
}
