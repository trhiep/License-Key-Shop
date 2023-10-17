using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class DepositRequestHe173252
    {
        public int RequestId { get; set; }
        public string UserUsername { get; set; } = null!;
        public double Amount { get; set; }
        public DateTime RequestDate { get; set; }

        public virtual UserHe173252 UserUsernameNavigation { get; set; } = null!;
    }
}
