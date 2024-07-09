using AutoMapper;
using BLL.Interfaces;
using BLL.Repositories;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PL.Helper;
using PL.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PL.Controllers
{
    //[Authorize("Admin")]  // Specific Role
    [Authorize] 
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        // private readonly EmployeeRepository employeeRepository;  // You develop against interface not concrete class


        public EmployeeController(/*EmployeeRepository _employeeRepository*/ IEmployeeRepository IemployeeRepository,
            IDepartmentRepository _departmentRepository, IMapper _mapper) // Ask CLR to Create Object From Class that implement IEmployeeRepository 
        {
            employeeRepository = IemployeeRepository;
            departmentRepository = _departmentRepository;
            mapper = _mapper;
            // employeeRepository = _employeeRepository;
            //employeeRepository = new EmployeeRepository(); // Invalid
        }
        public async Task<IActionResult> Index(string? Name)
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(Name))
                employees = await employeeRepository.GetAllAsync();
            else
                employees = await employeeRepository.GetEmployeesByNameAsync(Name);

            var MappedEmployees = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);


        }

        public async Task<IActionResult> Create()
        {
            ViewBag.departments = await departmentRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVm)
        {
            if (ModelState.IsValid)
            {
                string ImageName = DocumentSettings.UploadFile(employeeVm.Image, "Images");
                employeeVm.ImageName = ImageName;
                var MappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employeeVm);

                int Result = await employeeRepository.AddAsync(MappedEmployee);
                if (Result > 0)
                {
                    TempData["Message"] = "Employee has been Added";
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(employeeVm);
            }
        }

        //BaseURL/Details/id
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id == null) return BadRequest();
            var employee = await employeeRepository.GetByIdAsync(id.Value);
            if (employee is null) return NotFound();

            var MappedEmployee = mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(ViewName, MappedEmployee);


        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.departments = await departmentRepository.GetAllAsync();
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel employee, [FromRoute] int id)
        {
            if (id != employee.ID) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    if(employee.ImageName != null)
                    {
                    string ImageName = DocumentSettings.UploadFile(employee.Image, "Images");
                    employee.ImageName = ImageName;

                    }
                    var MappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employee);
                    int Result = employeeRepository.Update(MappedEmployee);
                    if (Result > 0)
                    {
                        TempData["Message"] = "Employee has been Updated";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                { ModelState.AddModelError(string.Empty, ex.Message); }

            }
            return View(employee);
        }

        public async Task<IActionResult> Delete(int? id) { return await Details(id, "Delete"); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(EmployeeViewModel employee, [FromRoute] int id)
        {
            if (id != employee.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employee);

                    int Result = employeeRepository.Delete(MappedEmployee);
                    if (Result > 0)
                    {
                        if (employee.ImageName != null) 
                            DocumentSettings.DeleteFile(employee.ImageName, "Images"); 

                        TempData["Message"] = "Employee has been Deleted";
                    }
                    return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employee);
        }



    }
}
