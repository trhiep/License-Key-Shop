using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class DepositHistoryHe173252
    {
        public int DepositId { get; set; }
        public string UserUsername { get; set; } = null!;
        public double Amount { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionBy { get; set; } = null!;

        public virtual UserHe173252 UserUsernameNavigation { get; set; } = null!;
    }
}
