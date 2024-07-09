using BLL.Interfaces;
using DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>  where T : class
    {
        private readonly MvcAppDbContext appdb;

        public GenericRepository(MvcAppDbContext _mvcAppDb)
        {
            appdb = _mvcAppDb;
        }
        public async Task<int> AddAsync(T entity)
        {
              appdb.Add(entity);
            return await appdb.SaveChangesAsync();
        }

        public int Delete(T entity)
        {
            appdb.Remove(entity);
            return appdb.SaveChanges();
        }

        public void Dispose()
        {
             appdb.Dispose();
        }

        //public IEnumerable<T> GetAll()
        //{
        //  return  appdb.Set<T>().ToList();
        //}

        public async Task<T> GetByIdAsync(int id)
        {
           return await appdb.Set<T>().FindAsync(id);
        }

        public int Update(T entity)
        {
           appdb.Update(entity);
            return appdb.SaveChanges();
        }
    }
}
