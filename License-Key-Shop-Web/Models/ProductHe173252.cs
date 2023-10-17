using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class ProductHe173252
    {
        public ProductHe173252()
        {
            CartItemHe173252s = new HashSet<CartItemHe173252>();
            ProductKeyHe173252s = new HashSet<ProductKeyHe173252>();
            ProductRateHe173252s = new HashSet<ProductRateHe173252>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public double Price { get; set; }
        public int Description { get; set; }
        public int CategoryCategoryId { get; set; }

        public virtual CategoryHe173252 CategoryCategory { get; set; } = null!;
        public virtual ICollection<CartItemHe173252> CartItemHe173252s { get; set; }
        public virtual ICollection<ProductKeyHe173252> ProductKeyHe173252s { get; set; }
        public virtual ICollection<ProductRateHe173252> ProductRateHe173252s { get; set; }
    }
}
