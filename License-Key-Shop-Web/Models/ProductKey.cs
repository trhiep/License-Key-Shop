using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class ProductKey
    {
        public int KeyId { get; set; }
        public int ProductProductId { get; set; }
        public string ProductKeyValue { get; set; } = null!;
        public string? ExpirationDate { get; set; }
        public bool IsExpired { get; set; }
        public string getFormatedKeyValue()
        {
            return this.ProductKeyValue.Substring(0, 5) + "....." + this.ProductKeyValue.Substring(this.ProductKeyValue.Length - 6);
        }

        public virtual Product ProductProduct { get; set; } = null!;
    }
}
