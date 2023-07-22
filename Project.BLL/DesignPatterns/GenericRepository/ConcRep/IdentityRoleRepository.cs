using Project.BLL.DesignPatterns.GenericRepository.IntRep;
using Project.BLL.DesignPatterns.SingletonPattern;
using Project.DAL.Context;
using Project.ENTITY.Enums;
using Project.ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.GenericRepository.ConcRep
{
    public class IdentityRoleRepository : IRepository<IdentityRole>
    {
        MyContext _db;

        public IdentityRoleRepository()
        {
            _db = DBTool.DbInstance;
        }

        protected void Save()
        {
            _db.SaveChanges();
        }

        public void Add(IdentityRole item)
        {
            _db.Set<IdentityRole>().Add(item);
            Save();
        }

        public bool Any(Expression<Func<IdentityRole, bool>> exp)
        {
            return _db.Set<IdentityRole>().Any(exp);
        }

        public void Delete(IdentityRole item)
        {
            item.DeletedDate = DateTime.Now;
            item.DataStatus = DataStatus.Silinmiş;
            Save();
        }

        public void DeleteRange(IEnumerable<IdentityRole> items)
        {
            foreach (var item in items)
            {
                item.DeletedDate = DateTime.Now;
                item.DataStatus = DataStatus.Silinmiş;
            }
            Save();
        }

        public void Destroy(IdentityRole item)
        {
            _db.Set<IdentityRole>().Remove(item);
        }

        public IdentityRole Find(int id)
        {
            return _db.Set<IdentityRole>().Find(id);
        }

        public IdentityRole FirstOrDefault(Expression<Func<IdentityRole, bool>> exp)
        {
            return _db.Set<IdentityRole>().FirstOrDefault(exp);
        }

        public List<IdentityRole> GetActives()
        {
            return Where(x => x.DataStatus != DataStatus.Silinmiş);
        }

        public List<IdentityRole> GetAll()
        {
            return _db.Set<IdentityRole>().ToList();
        }

        public List<IdentityRole> GetModifieds()
        {
            return Where(x => x.DataStatus == DataStatus.Güncellenmiş);
        }

        public List<IdentityRole> GetPassives()
        {
            return Where(x => x.DataStatus == DataStatus.Silinmiş);
        }

        public object Select(Expression<Func<IdentityRole, bool>> exp)
        {
            return _db.Set<IdentityRole>().Select(exp).ToList();
        }

        public void Update(IdentityRole item)
        {
            item.ModifiedDate = DateTime.Now;
            item.DataStatus = DataStatus.Güncellenmiş;
            IdentityRole toBeUpdated = Find(item.Id);
            _db.Entry(toBeUpdated).CurrentValues.SetValues(item);
            Save();
        }

        public List<IdentityRole> Where(Expression<Func<IdentityRole, bool>> exp)
        {
            return _db.Set<IdentityRole>().Where(exp).ToList();
        }
    }
}
