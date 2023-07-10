using Microsoft.AspNetCore.Identity;
using Project.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITY.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus DataStatus { get; set; }

        //Relational Properties
        [ForeignKey("Company")]
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<ServiceTicket> ServiceTickets { get; set; }

        public virtual ICollection<UserServiceTicket> UserServiceTickets { get; set; }

        public User()
        {
            CreatedDate = DateTime.Now;
            DataStatus = DataStatus.Girilmiş;
        }
    }
}
