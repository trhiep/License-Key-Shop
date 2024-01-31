using System;
using System.Collections.Generic;
using System.Text;

namespace License_Key_Shop_Web.Models
{
    public partial class Product
    {
        public Product()
        {
            CartItems = new HashSet<CartItem>();
            ProductKeys = new HashSet<ProductKey>();
            ProductRates = new HashSet<ProductRate>();
            ProductSubImages = new HashSet<ProductSubImage>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Image { get; set; } = null!;
        public double Price { get; set; }
        public string Description { get; set; } = null!;
        public int CategoryCategoryId { get; set; }
        public float? DiscountRate { get; set; }
        public bool IsSelling { get; set; }
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

        public virtual Category CategoryCategory { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<ProductKey> ProductKeys { get; set; }
        public virtual ICollection<ProductRate> ProductRates { get; set; }
        public virtual ICollection<ProductSubImage> ProductSubImages { get; set; }
    }
}
