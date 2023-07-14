using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITY.Models
{
    public class ServiceTicketImage : BaseEntity
    {
        public string ImageUrl { get; set; }

        //Relational Properties

        [ForeignKey("ServiceTicket")]
        public int ServiceTicketID { get; set; }
        public virtual ServiceTicket ServiceTicket { get; set; }
    }
}
