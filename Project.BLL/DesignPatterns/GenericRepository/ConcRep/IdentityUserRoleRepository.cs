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
    public class IdentityUserRoleRepository : IRepository<IdentityUserRole>
    {
        MyContext _db;

        public IdentityUserRoleRepository()
        {
            _db = DBTool.DbInstance;
        }

        protected void Save()
        {
            _db.SaveChanges();
        }

        public void Add(IdentityUserRole item)
        {
            _db.Set<IdentityUserRole>().Add(item);
            Save();
        }

        public bool Any(Expression<Func<IdentityUserRole, bool>> exp)
        {
            return _db.Set<IdentityUserRole>().Any(exp);
        }

        public void Destroy(IdentityUserRole item)
        {
            _db.Set<IdentityUserRole>().Remove(item);
        }

        public IdentityUserRole Find(int id)
        {
            return _db.Set<IdentityUserRole>().Find(id);
        }

        public IdentityUserRole FirstOrDefault(Expression<Func<IdentityUserRole, bool>> exp)
        {
            return _db.Set<IdentityUserRole>().FirstOrDefault(exp);
        }

        public List<IdentityUserRole> GetAll()
        {
            return _db.Set<IdentityUserRole>().ToList();
        }

        public object Select(Expression<Func<IdentityUserRole, bool>> exp)
        {
            return _db.Set<IdentityUserRole>().Select(exp).ToList();
        }

        public void Update(IdentityUserRole item)
        {

        }

        public List<IdentityUserRole> Where(Expression<Func<IdentityUserRole, bool>> exp)
        {
            return _db.Set<IdentityUserRole>().Where(exp).ToList();
        }

        public List<IdentityUserRole> GetActives()
        {
            throw new NotImplementedException();
        }

        public List<IdentityUserRole> GetPassives()
        {
            throw new NotImplementedException();
        }

        public List<IdentityUserRole> GetModifieds()
        {
            throw new NotImplementedException();
        }

        public void Delete(IdentityUserRole item)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<IdentityUserRole> items)
        {
            throw new NotImplementedException();
        }
    }
}
