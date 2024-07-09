using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGenericRepository<T>:IDisposable
    {
        //IEnumerable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task<int> AddAsync(T entity);
        int Update(T entity);
        int Delete(T entity);

        void Dispose();

    }
}
