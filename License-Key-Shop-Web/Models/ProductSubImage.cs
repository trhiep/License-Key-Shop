using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class ProductSubImage
    {
        public int SubImageId { get; set; }
        public int ProductProductId { get; set; }
        public string Image { get; set; } = null!;

        public virtual Product ProductProduct { get; set; } = null!;
    }
}
