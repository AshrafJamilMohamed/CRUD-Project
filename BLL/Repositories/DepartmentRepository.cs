using BLL.Interfaces;
using DAL.Contexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class DepartmentRepository :  GenericRepository<Department> ,IDepartmentRepository 
    {
        private readonly MvcAppDbContext context;

        /*
الكلاس ده بيعمل كونستراكتور تشيننج علي الباز كونتستراكتور بتاع الجينارك كلاس اللي هو اصلا بيتحقن 
ب اوبجكت من الكلاس بتاع الداتا بيز وعشان احل المشكله دي انا قدمت بس عمليه تعريف الاوبجكت ده هنا و هبعته از ا براميتر 
للجينارك كلاس بس هو كده مش هيروح يعمل اوبجكت تاني لا وكذلك الامر بالنسبه لكلاس الامبلويي

*/


        public DepartmentRepository(MvcAppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
           return await context.Departments.ToListAsync();
        }
    }
}
