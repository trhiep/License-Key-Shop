using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class ProductRateHe173252
    {
        public int RateId { get; set; }
        public string UserUsername { get; set; } = null!;
        public int ProductProductId { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime RateDate { get; set; }

        public virtual ProductHe173252 ProductProduct { get; set; } = null!;
        public virtual UserHe173252 UserUsernameNavigation { get; set; } = null!;
    }
}
