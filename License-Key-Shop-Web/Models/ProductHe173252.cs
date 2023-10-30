using System;
using System.Collections.Generic;
using System.Text;

namespace License_Key_Shop_Web.Models
{
    public partial class ProductHe173252
    {
        public ProductHe173252()
        {
            CartItemHe173252s = new HashSet<CartItemHe173252>();
            ProductKeyHe173252s = new HashSet<ProductKeyHe173252>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Image { get; set; } = null!;
        public double Price { get; set; }
        public string Description { get; set; } = null!;
        public int CategoryCategoryId { get; set; }

        public string GetFormattedPrice()
        {
            string amount = this.Price.ToString();
            StringBuilder formattedAmount = new StringBuilder();
            int endPoint = amount.IndexOf(".");
            if (endPoint < 0)
            {
                endPoint = amount.Length;
            }
            int count = 0;
            for (int i = endPoint - 1; i >= 0; i--)
            {
                formattedAmount.Insert(0, amount[i]);
                count++;

                if (count % 3 == 0 && i > 0)
                {
                    formattedAmount.Insert(0, ",");
                }
            }
            if (endPoint == amount.Length)
            {
                return formattedAmount.ToString();
            }
            else
            {
                return formattedAmount.ToString() + amount.Substring(endPoint);
            }
        }

        public virtual CategoryHe173252 CategoryCategory { get; set; } = null!;
        public virtual ICollection<CartItemHe173252> CartItemHe173252s { get; set; }
        public virtual ICollection<ProductKeyHe173252> ProductKeyHe173252s { get; set; }
    }
}
