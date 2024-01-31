using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class OrderHistory
    {
        public OrderHistory()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public string UserUsername { get; set; } = null!;

        public virtual User UserUsernameNavigation { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
