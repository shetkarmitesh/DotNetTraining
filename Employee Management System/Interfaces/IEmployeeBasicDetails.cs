using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.Interfaces
{
    public interface IEmployeeBasicDetails
    {
        Task<EmployeeBasicDetailsDTO> AddEmployeeBasicDetails(EmployeeBasicDetailsDTO basicDetailsDTO);
        Task<List<EmployeeBasicDetailsDTO>> GetAllEmployeeBasicDetails();
        Task<EmployeeBasicDetailsDTO> GetEmployeeBasicDetailsById(string id);
 /*       Task<EmployeeBasicDetailsDTO> GetEmployeeBasicDetailsByUId(string uId);*/
        Task<EmployeeBasicDetailsDTO> UpdateEmployeeBasicDetails(EmployeeBasicDetailsDTO basicDetailsDTO);
        Task<String> DeleteEmployeeBasicDetailsById(string id);
    }
}
