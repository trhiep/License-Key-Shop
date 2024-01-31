using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class BalanceHistory
    {
        public int Id { get; set; }
        public string UserUsername { get; set; } = null!;
        public bool Status { get; set; }
        public double Amount { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime ChangeDate { get; set; }
        public double NewBalance { get; set; }

        public virtual User UserUsernameNavigation { get; set; } = null!;
    }
}
