using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class User
    {
        public User()
        {
            BalanceHistories = new HashSet<BalanceHistory>();
            DepositHistories = new HashSet<DepositHistory>();
            OrderHistories = new HashSet<OrderHistory>();
            ProductRates = new HashSet<ProductRate>();
        }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int RoleRoleId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public string? UserImage { get; set; }

        public virtual Role RoleRole { get; set; } = null!;
        public virtual Cart? Cart { get; set; }
        public virtual UserBalance? UserBalance { get; set; }
        public virtual ICollection<BalanceHistory> BalanceHistories { get; set; }
        public virtual ICollection<DepositHistory> DepositHistories { get; set; }
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
        public virtual ICollection<ProductRate> ProductRates { get; set; }
    }
}
