using Microsoft.EntityFrameworkCore;
using Project.BLL.DesignPatterns.GenericRepository.BaseRep;
using Project.BLL.DesignPatterns.SingletonPattern;
using Project.DAL.Context;
using Project.ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.GenericRepository.ConcRep
{
    public class ServiceTicketRepository : BaseRepository<ServiceTicket>
    {
        MyContext _db;

        public ServiceTicketRepository()
        {
            _db = DBTool.DbInstance;
        }

        protected void Save()
        {
            _db.SaveChanges();
        }

        public List<ServiceTicket> GetAllActiveRelated()
        {
            return _db.Set<ServiceTicket>().Include(x => x.User).Include(x=>x.User.Company).Where(x=>x.DataStatus != ENTITY.Enums.DataStatus.Silinmiş).ToList();
        }

        public List<ServiceTicket> GetDeletedRelated()
        {
            return _db.Set<ServiceTicket>().Include(x => x.User).Include(x => x.User.Company).Where(x => x.DataStatus == ENTITY.Enums.DataStatus.Silinmiş).ToList();
        }

    }
}
