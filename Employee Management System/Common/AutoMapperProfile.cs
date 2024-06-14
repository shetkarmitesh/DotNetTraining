using AutoMapper;
using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;

namespace Employee_Management_System.Common
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EmployeeBasicDetailsDTO, EmployeeBasicDetails>().ReverseMap();
            CreateMap<EmployeeAdditionalDetailsDTO, EmployeeAdditionalDetails>().ReverseMap();
        }
    }
}
