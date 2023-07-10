using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITY.Models
{
    public class Company : BaseEntity
    {
        public string CompanyName { get; set; }
        public string CompanyRepresentative { get; set; }


        //Relational Properties

        public virtual ICollection<User> Users { get; set; }


    }
}
