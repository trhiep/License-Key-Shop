﻿using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class ProductKeyHe173252
    {
        public int KeyId { get; set; }
        public int ProductProductId { get; set; }
        public string ProductKey { get; set; } = null!;
        public string? ExpirationDate { get; set; }
        public bool IsExpired { get; set; }

        public virtual ProductHe173252 ProductProduct { get; set; } = null!;
    }
}
