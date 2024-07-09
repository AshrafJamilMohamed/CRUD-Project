using AutoMapper;
using DAL.Models;
using PL.ViewModels;

namespace PL.ModelsProfile
{
    public class EmployeeProfile :Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee,EmployeeViewModel>().ReverseMap();
        }
    }
}
