using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class UserBalanceHe173252
    {
        public string UserUsername { get; set; } = null!;
        public double Amount { get; set; }

        public virtual UserHe173252 UserUsernameNavigation { get; set; } = null!;
    }
}
