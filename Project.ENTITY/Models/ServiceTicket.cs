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
    public class ServiceTicket : BaseEntity
    {
        public string Task { get; set; }
        public string Description { get; set; }
        public Priority TaskPriority { get; set; }
        public Enums.TaskStatus TaskStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }

        //Relational Properties
        public virtual ICollection<UserServiceTicket> UserServiceTickets { get; set; }

        [ForeignKey("User")]
        public int? CreatedByID { get; set; }
        public virtual User User { get; set; }

        public ServiceTicket()
        {
            TaskStatus = Enums.TaskStatus.Beklemede;
        }
    }
}
