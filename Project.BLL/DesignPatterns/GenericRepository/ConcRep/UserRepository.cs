using Project.BLL.DesignPatterns.GenericRepository.BaseRep;
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
    public class UserRepository : IRepository<User>
    {
        MyContext _db;

        public UserRepository()
        {
            _db = DBTool.DbInstance;
        }

        protected void Save()
        {
            _db.SaveChanges();
        }


        public void Add(User item)
        {
            _db.Set<User>().Add(item);
            Save();
        }

        public bool Any(Expression<Func<User, bool>> exp)
        {
            return _db.Set<User>().Any(exp);
        }

        public void Delete(User item)
        {
            item.DeletedDate = DateTime.Now;
            item.DataStatus = DataStatus.Silinmiş;
            Save();
        }

        public void Destroy(User item)
        {
            _db.Set<User>().Remove(item);
        }

        public User Find(int id)
        {
            return _db.Set<User>().Find(id);
        }

        public User FirstOrDefault(Expression<Func<User, bool>> exp)
        {
            return _db.Set<User>().FirstOrDefault(exp);
        }

        public List<User> GetActives()
        {
            return Where(x => x.DataStatus != DataStatus.Silinmiş);
        }

        public List<User> GetAll()
        {
            return _db.Set<User>().ToList();
        }

        public List<User> GetModifieds()
        {
            return Where(x => x.DataStatus == DataStatus.Güncellenmiş);
        }

        public List<User> GetPassives()
        {
            return Where(x => x.DataStatus == DataStatus.Silinmiş);
        }

        public object Select(Expression<Func<User, bool>> exp)
        {
            return _db.Set<User>().Select(exp).ToList();
        }

        public void Update(User item)
        {
            item.ModifiedDate = DateTime.Now;
            item.DataStatus = DataStatus.Güncellenmiş;
            User toBeUpdated = Find(item.Id);
            _db.Entry(toBeUpdated).CurrentValues.SetValues(item);
            Save();
        }

        public List<User> Where(Expression<Func<User, bool>> exp)
        {
            return _db.Set<User>().Where(exp).ToList();
        }

        public void DeleteRange(IEnumerable<User> items)
        {
            foreach (var item in items)
            {
                item.DeletedDate = DateTime.Now;
                item.DataStatus = DataStatus.Silinmiş;
            }
            Save();
        }
    }
}
