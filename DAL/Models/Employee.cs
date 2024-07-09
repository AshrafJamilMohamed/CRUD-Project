using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Employee
    {
        public int ID  { get; set; } //PK

        [Required]
        [MaxLength(50)]
       
        public string Name { get; set; }

        [Required]
     
 
        public string Address { get; set; }

        [Required]
        public string  Email { get; set; }
       
        [MaxLength(11)]
       
        // [Phone]
        public string Phone { get; set; }

        public DateTime DateOfCreation { get; set; } = DateTime.Now;

        public DateTime HiringDate  { get; set; }

        public int? Age { get; set; }

        public decimal Salary { get; set; }

        public string ImageName { get; set; }

        public bool IsActive { get; set; }
        [ForeignKey("Department")]


        public int? DepartmentId { get; set; }

        [InverseProperty("Employees")]
        public Department Department { get; set; }

    }
}
