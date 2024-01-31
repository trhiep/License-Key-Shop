using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class ProductRate
    {
        public int RateId { get; set; }
        public string UserUsername { get; set; } = null!;
        public int ProductProductId { get; set; }
        public int Point { get; set; }
        public string Comment { get; set; } = null!;

        public virtual Product ProductProduct { get; set; } = null!;
        public virtual User UserUsernameNavigation { get; set; } = null!;
    }
}
