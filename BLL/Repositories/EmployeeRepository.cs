using BLL.Interfaces;
using DAL.Contexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        // Don't Open the Connection if you don't need 
        private readonly MvcAppDbContext dbContext;

        public EmployeeRepository(MvcAppDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await dbContext.Employees.Include(E => E.Department).ToListAsync();
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
            => dbContext.Employees.Include(E=>E.Department).Where(E => E.Address == address);

        public async Task<IEnumerable<Employee>> GetEmployeesByNameAsync(string Name)
        {
            return await dbContext.Employees.Include(E => E.Department).Where(E => E.Name.ToLower().Contains(Name.ToLower())).ToListAsync();
        }
    }
}
