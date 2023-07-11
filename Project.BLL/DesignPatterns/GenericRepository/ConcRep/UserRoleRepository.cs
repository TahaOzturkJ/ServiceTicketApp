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
    public class UserRoleRepository : IRepository<UserRole>
    {
        MyContext _db;

        public UserRoleRepository()
        {
            _db = DBTool.DbInstance;
        }

        protected void Save()
        {
            _db.SaveChanges();
        }


        public void Add(UserRole item)
        {
            _db.Set<UserRole>().Add(item);
            Save();
        }

        public bool Any(Expression<Func<UserRole, bool>> exp)
        {
            return _db.Set<UserRole>().Any(exp);
        }

        public void Destroy(UserRole item)
        {
            _db.Set<UserRole>().Remove(item);
        }

        public UserRole Find(int id)
        {
            return _db.Set<UserRole>().Find(id);
        }

        public UserRole FirstOrDefault(Expression<Func<UserRole, bool>> exp)
        {
            return _db.Set<UserRole>().FirstOrDefault(exp);
        }

        public List<UserRole> GetAll()
        {
            return _db.Set<UserRole>().ToList();
        }

        public object Select(Expression<Func<UserRole, bool>> exp)
        {
            return _db.Set<UserRole>().Select(exp).ToList();
        }

        public void Update(UserRole item)
        {
            UserRole toBeUpdated = Find(item.UserId);
            _db.Entry(toBeUpdated).CurrentValues.SetValues(item);
            Save();
        }

        public List<UserRole> Where(Expression<Func<UserRole, bool>> exp)
        {
            return _db.Set<UserRole>().Where(exp).ToList();
        }

        public List<UserRole> GetActives()
        {
            throw new NotImplementedException();
        }

        public List<UserRole> GetPassives()
        {
            throw new NotImplementedException();
        }

        public List<UserRole> GetModifieds()
        {
            throw new NotImplementedException();
        }

        public void Delete(UserRole item)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<UserRole> items)
        {
            throw new NotImplementedException();
        }
    }
}
