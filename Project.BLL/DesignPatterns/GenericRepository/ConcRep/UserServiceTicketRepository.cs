using Microsoft.EntityFrameworkCore;
using Project.BLL.DesignPatterns.GenericRepository.BaseRep;
using Project.BLL.DesignPatterns.SingletonPattern;
using Project.DAL.Context;
using Project.ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.GenericRepository.ConcRep
{
    public class UserServiceTicketRepository : BaseRepository<UserServiceTicket>
    {
        MyContext _db;

        public UserServiceTicketRepository()
        {
            _db = DBTool.DbInstance;
        }

        protected void Save()
        {
            _db.SaveChanges();
        }

        public List<UserServiceTicket> GetAllRelated()
        {
            return _db.Set<UserServiceTicket>().Include(x=>x.User).Include(y=>y.ServiceTicket).Where(x => x.ServiceTicket.DataStatus != ENTITY.Enums.DataStatus.Silinmiş).Include(y=>y.User.Company).ToList();
        }
    }
}
