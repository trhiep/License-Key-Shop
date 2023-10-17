﻿using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class OrderDetailHe173252
    {
        public int ProductSoldId { get; set; }
        public int OrderHistoryOrderId { get; set; }
        public string ProductKey { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }

        public virtual OrderHistoryHe173252 OrderHistoryOrder { get; set; } = null!;
    }
}
