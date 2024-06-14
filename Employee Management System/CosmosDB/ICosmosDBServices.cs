using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.CosmosDB
{
    public interface ICosmosDBServices
    {
        Task<EmployeeAdditionalDetails> AddEmployeeAdditionalDetails(EmployeeAdditionalDetails additionalDetails);
        Task<List<EmployeeAdditionalDetails>> GetAllEmployeeAdditionalDetails();
        Task<EmployeeAdditionalDetails> GetAllEmployeeAdditionalDetailsById(string id);
        Task<EmployeeAdditionalDetails> UpdateEmployeeAdditionalDetails(EmployeeAdditionalDetails additionalDetails);


        Task<EmployeeBasicDetails> AddEmployeeBasicDetails(EmployeeBasicDetails basicDetails);
        Task<List<EmployeeBasicDetails>> GetAllEmployeeBasicDetails();
        Task<EmployeeBasicDetails> GetEmployeeBasicDetailsById(string id);
        Task<EmployeeBasicDetails> GetEmployeeBasicDetailsByUId(string uId);
        Task<EmployeeBasicDetails> UpdateEmployeeBasicDetails(EmployeeBasicDetails basicDetails);
        
    }
}
