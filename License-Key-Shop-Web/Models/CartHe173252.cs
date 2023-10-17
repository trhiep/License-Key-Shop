using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class CartHe173252
    {
        public CartHe173252()
        {
            CartItemHe173252s = new HashSet<CartItemHe173252>();
        }

        public string UserUsername { get; set; } = null!;
        public double Total { get; set; }

        public virtual UserHe173252 UserUsernameNavigation { get; set; } = null!;
        public virtual ICollection<CartItemHe173252> CartItemHe173252s { get; set; }
    }
}
