using Microsoft.AspNetCore.Identity;
using Project.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITY.Models
{
    public class UserRole : IdentityUserRole<int>
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus DataStatus { get; set; }
    }
}
