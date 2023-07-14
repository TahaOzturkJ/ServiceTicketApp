using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITY.Models
{
    public class ServiceTicketCommentImage : BaseEntity
    {
        public string ImageUrl { get; set; }

        //Relational Properties

        [ForeignKey("ServiceTicketComment")]
        public int ServiceTicketCommentID { get; set; }
        public virtual ServiceTicketComment ServiceTicketComment { get; set; }

    }
}
