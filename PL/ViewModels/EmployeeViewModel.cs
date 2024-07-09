using DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int ID { get; set; } //PK

        [Required(ErrorMessage = "Name is Required with 50 Max & 5 Min Length")]
        [MaxLength(50)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        // Pattern => 123-street-city-country
        // ^ => Means start , $ => Means End 
        [RegularExpression("^[0-9]{1,4}-[a-zA-Z]{1,6}-[a-zA-Z]{1,5}-[a-zA-Z]{3,10}$", ErrorMessage = "ex 1234-street-city-country")]

        public string Address { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [MaxLength(11)]
        [MinLength(10)]
        // [Phone]
        public string Phone { get; set; }

      
        public DateTime HiringDate { get; set; }

        [Range(22, 25, ErrorMessage = "The age must be Within 22 and 25")]

        public int? Age { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public IFormFile  Image { get; set; }

        public string  ImageName { get; set; }

        public bool IsActive { get; set; }
        [ForeignKey("Department")]


        public int? DepartmentId { get; set; }

        [InverseProperty("Employees")]
        public Department Department { get; set; }

    }
}
