using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Employee_Management_System.Interfaces
{
    public interface IEmployeeAdditionalDetails
    {
        Task<EmployeeAdditionalDetailsDTO> AddEmployeeAdditionalDetails(EmployeeAdditionalDetailsDTO additionalDetailsDTO);
        Task<List<EmployeeAdditionalDetailsDTO>> GetAllEmployeeAdditionalDetails();
        Task<EmployeeAdditionalDetailsDTO> GetAllEmployeeAdditionalDetailsById(string id);
        Task<EmployeeAdditionalDetailsDTO> UpdateEmployeeAdditionalDetails(EmployeeAdditionalDetailsDTO additionalDetailsDTO);
        Task<String> DeleteEmployeeAdditionalDetailsById(string id);

        Task<IActionResult> AddEmployeeAdditionalDetailByMakePostRequest(EmployeeAdditionalDetailsDTO employeeAdditionalDetailsDTO)
    }
}
