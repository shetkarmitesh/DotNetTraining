using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.Controllers
{
        [Route("api/[Controller]/[Action]")]
        [ApiController]
    public class EmployeeAdditionalDetailsController : Controller
    {
        private readonly IEmployeeAdditionalDetails _employeeAdditionalDetails;
        public EmployeeAdditionalDetailsController(IEmployeeAdditionalDetails employeeAdditionalDetails)
        {
            _employeeAdditionalDetails = employeeAdditionalDetails;
        }
        [HttpPost]
        public async Task<EmployeeAdditionalDetailsDTO> AddEmployeeAdditionalDetails(EmployeeAdditionalDetailsDTO additionalDetailsDTO)
        {
            return await _employeeAdditionalDetails.AddEmployeeAdditionalDetails(additionalDetailsDTO);
        }

        [HttpGet]
        public async Task<List<EmployeeAdditionalDetailsDTO>> GetAllEmployeeAdditionalDetails()
        {
            return await _employeeAdditionalDetails.GetAllEmployeeAdditionalDetails();
        }

        [HttpGet]
        public async Task<EmployeeAdditionalDetailsDTO> GetEmployeeAdditionalDetailsById(string id)
        {
            return await _employeeAdditionalDetails.GetAllEmployeeAdditionalDetailsById(id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployeeAdditionalDetails(EmployeeAdditionalDetailsDTO additionalDetailsDTO)
        {
            try
            {
                var updatedEmployee = await _employeeAdditionalDetails.UpdateEmployeeAdditionalDetails(additionalDetailsDTO);
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Updating Additional Employee Details : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        public async Task<String> DeleteEmployeeAdditionalDetailsById(string id)
        {
            var response = await _employeeAdditionalDetails.DeleteEmployeeAdditionalDetailsById(id);
            return response;
        }
    }
}
