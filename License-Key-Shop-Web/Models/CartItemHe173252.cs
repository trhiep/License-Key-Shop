using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class CartItemHe173252
    {
        public int ItemId { get; set; }
        public string UserUsername { get; set; } = null!;
        public int ProductProductId { get; set; }
        public int Quantity { get; set; }

        public virtual ProductHe173252 ProductProduct { get; set; } = null!;
        public virtual CartHe173252 UserUsernameNavigation { get; set; } = null!;
    }
}
