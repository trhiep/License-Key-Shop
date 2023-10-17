using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class UserHe173252
    {
        public UserHe173252()
        {
            BalanceHistoryHe173252s = new HashSet<BalanceHistoryHe173252>();
            DepositRequestHe173252s = new HashSet<DepositRequestHe173252>();
            OrderHistoryHe173252s = new HashSet<OrderHistoryHe173252>();
            ProductRateHe173252s = new HashSet<ProductRateHe173252>();
            UserBalanceHe173252s = new HashSet<UserBalanceHe173252>();
        }

        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int RoleRoleId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }

        public virtual RoleHe173252 RoleRole { get; set; } = null!;
        public virtual CartHe173252? CartHe173252 { get; set; }
        public virtual ICollection<BalanceHistoryHe173252> BalanceHistoryHe173252s { get; set; }
        public virtual ICollection<DepositRequestHe173252> DepositRequestHe173252s { get; set; }
        public virtual ICollection<OrderHistoryHe173252> OrderHistoryHe173252s { get; set; }
        public virtual ICollection<ProductRateHe173252> ProductRateHe173252s { get; set; }
        public virtual ICollection<UserBalanceHe173252> UserBalanceHe173252s { get; set; }
    }
}
