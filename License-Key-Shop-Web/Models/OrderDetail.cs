using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderHistoryOrderId { get; set; }
        public string ProductSoldName { get; set; } = null!;
        public string ProductKey { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }

        public virtual OrderHistory OrderHistoryOrder { get; set; } = null!;
    }
}
