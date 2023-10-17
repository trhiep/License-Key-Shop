using System;
using System.Collections.Generic;

namespace License_Key_Shop_Web.Models
{
    public partial class RoleHe173252
    {
        public RoleHe173252()
        {
            UserHe173252s = new HashSet<UserHe173252>();
        }

        public int RoleId { get; set; }
        public int RoleName { get; set; }

        public virtual ICollection<UserHe173252> UserHe173252s { get; set; }
    }
}
