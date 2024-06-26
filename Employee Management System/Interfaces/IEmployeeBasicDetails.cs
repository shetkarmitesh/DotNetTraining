using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Microsoft.AspNetCore.Mvc;
using VisitorSecurityClearanceSystem.DTOs;

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


        Task<VisitorDTO> AddVisitorByMakePostRequest(VisitorDTO visitor);
        Task <IEnumerable<VisitorDTO>> GetVisitorByMakePostRequest();

        Task<EmployeeBasicDetailsDTO> AddEmployeeBasicDetailsByMakePostRequest(EmployeeBasicDetailsDTO employeeBasicDetailsDto);
        Task<IEnumerable<EmployeeBasicDetailsDTO>> GetEmployeeBasicDetailsByMakeGetRequest();

        Task<EmployeeFilterCriteria> GetAllEmployeesByPagination(EmployeeFilterCriteria employeeFilterCriteria);
        Task<List<EmployeeBasicDetailsDTO>> GetAllEmployeeBasicDetailsByRole(string role);
    }
}
