using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITY.Models
{
    public class UserRoleInfo
    {
        public int UserId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

}
