using Project.ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.GenericRepository.IntRep
{
    public interface IRepository<T>
    {
        //List Commands: Sorgulama yapmak için kullanılan komutlar
        List<T> GetAll(); // Bütün bilgileri sorgular
        List<T> GetActives(); // Aktif verileri sorgular
        List<T> GetPassives(); // Pasif verileri sorgular
        List<T> GetModifieds(); // Güncellenmiş verileri sorgular

        //Modify Commands: Değişiklik yapmak için kullanılan komutlar
        void Add(T item); // ekleme metodu
        void Update(T item); // veriyi güncelleme metodu
        void Delete(T item); // veriyi pasife çeken metod
        void DeleteRange(IEnumerable<T> items); //Liste halindeki verileri pasife çeken metod
        void Destroy(T item); // veriyi yok eden metod


        //Linq Expressions: Linq Sorguları
        List<T> Where(Expression<Func<T, bool>> exp);
        bool Any(Expression<Func<T, bool>> exp);
        T FirstOrDefault(Expression<Func<T, bool>> exp);
        object Select(Expression<Func<T, bool>> exp);

        //Find: Primary Key'e göre veri sorgulayan ve döndüren metod
        T Find(int id);
    }
}
