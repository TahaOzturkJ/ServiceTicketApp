using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITY.Models
{
    public class ServiceTicketComment : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }


        //Relational Properties

        [ForeignKey("ServiceTicket")]
        public int ServiceTicketID { get; set; }
        public virtual ServiceTicket ServiceTicket { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<ServiceTicketCommentImage> ServiceTicketCommentImages { get; set; }

    }
}
