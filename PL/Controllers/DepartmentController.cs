using BLL.Interfaces;
using BLL.Repositories;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize]
    //[AllowAnonymous] // Default
    public class DepartmentController : Controller
    {
        //private readonly DepartmentRepository departmentRepository;   // You develop against interface not concrete class
        // Tester will make MockRepository class to test business logic in interface So that
        // you make ref from interface that can pointer on any class that implement this interface
        // So you don't need to change the ref [IDepartmentRepository]


        private readonly IDepartmentRepository departmentRepository;
        public DepartmentController(IDepartmentRepository _departmentRepository) // Ask CLR for Creating object from class implement interface IDepartmentRepository
                                                                                 // you must allow Dependency Injection in ConfigurationService fun in startup class 
        {
            departmentRepository = _departmentRepository;
            //departmentRepository=new DepartmentRepository(); // Invalid as this ctor depend on object from MvcAppDbContext class which CLR will make it not me ya ghalyyy
        }


        // Master Page of any Controller
        // BaseURL/Department/index
        public async Task<IActionResult> Index()
        {
            var Departments = await departmentRepository.GetAllAsync();
            //var Departments =  departmentRepository.GetAllAsync().Result; // Transfer Fun From [Async] To [Sync]
            return View(Departments);
        }

        [HttpGet]   // By Default
        public IActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                int Result = await departmentRepository.AddAsync(department);

                if (Result > 0)
                {
                    TempData["Message"] = "Department is Created";

                }
                return RedirectToAction(nameof(Index));
            }
            else
            { return View(department); }
        }


        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null) { return BadRequest(); } // Status code 400
            var department = await departmentRepository.GetByIdAsync(id.Value);
            if (department is null)
                return NotFound();
            return View(ViewName, department);
        }
        [HttpGet] // By Default



        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null) { return BadRequest(); }
            //var department = departmentRepository.GetById(id.Value);
            //if (department is null)
            //    return NotFound();
            return await Details(id, "Edit");

        }
        [HttpPost]
        [ValidateAntiForgeryToken] // To prevent any request from any other tool like [PostMan]
        public IActionResult Edit(Department department, [FromRoute] int id)
        {
            if (id != department.Id) { return NotFound(); } // More Security
            if (ModelState.IsValid)
            {
                try
                {

                   int Result= departmentRepository.Update(department);
                    if (Result > 0)
                    {
                        TempData["Message"] = "Department is Updated";

                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    // Log Exception
                    // Show Exception
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) { return BadRequest(); }
            //var department =departmentRepository.GetById(id.Value); 
            //if (department is null)
            //    return NotFound();           
            //return View(department);
            return await Details(id, "Delete");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Department department, [FromRoute] int id)
        {
            if (id != department.Id) { return BadRequest(); }

            if (ModelState.IsValid)
            {
                try
                {
                    int Result=  departmentRepository.Delete(department);

                    if (Result > 0)
                    {
                        TempData["Message"] = "Department is Deleted";

                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);

        }
    }
}
