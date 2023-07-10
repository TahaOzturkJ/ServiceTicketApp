using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITY.Models
{
    public class UserServiceTicket : BaseEntity
    {
        [ForeignKey("User")]
        public int UserID { get; set; }
        [ForeignKey("ServiceTicket")]
        public int ServiceTicketID { get; set; }

        //Relational Properties

        public virtual User User { get; set; }
        public virtual ServiceTicket ServiceTicket { get; set; }
    }
}
