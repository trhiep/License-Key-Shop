using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class ProductKeyHe173252
    {
        public int KeyId { get; set; }
        public int ProductProductId { get; set; }
        public string ProductKey { get; set; } = null!;
        public string? ExpirationDate { get; set; }
        public bool IsExpired { get; set; }
        public string getFormatedKeyValue()
        {
            return this.ProductKey.Substring(0, 5) + "....." + this.ProductKey.Substring(this.ProductKey.Length - 6);
        }

        public virtual ProductHe173252 ProductProduct { get; set; } = null!;
    }
}
