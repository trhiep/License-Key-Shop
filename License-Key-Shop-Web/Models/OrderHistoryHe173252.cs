using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class OrderHistoryHe173252
    {
        public OrderHistoryHe173252()
        {
            OrderDetailHe173252s = new HashSet<OrderDetailHe173252>();
        }

        public int OrderId { get; set; }
        public string UserUsername { get; set; } = null!;

        public virtual UserHe173252 UserUsernameNavigation { get; set; } = null!;
        public virtual ICollection<OrderDetailHe173252> OrderDetailHe173252s { get; set; }
    }
}
