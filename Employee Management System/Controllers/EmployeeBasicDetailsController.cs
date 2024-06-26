using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;
using Employee_Management_System.ServiceFilter;
using Employee_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using VisitorSecurityClearanceSystem.DTOs;

namespace Employee_Management_System.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class EmployeeBasicDetailsController : Controller
    {
        private readonly IEmployeeBasicDetails _employeeBasicDetails;

        public EmployeeBasicDetailsController(IEmployeeBasicDetails employeeBasicDetails)
        {
            _employeeBasicDetails = employeeBasicDetails;
        }

        [HttpPost]
        public async Task<EmployeeBasicDetailsDTO> AddEmployeeBasicDetails(EmployeeBasicDetailsDTO basicDetailsDTO)
        {
            return await _employeeBasicDetails.AddEmployeeBasicDetails(basicDetailsDTO);
        }

        [HttpGet]
        public async Task<List<EmployeeBasicDetailsDTO>> GetAllEmployeeBasicDetails()
        {
            return await _employeeBasicDetails.GetAllEmployeeBasicDetails();
        }

        [HttpGet]
        public async Task<EmployeeBasicDetailsDTO> GetEmployeeBasicDetailsById(string id)
        {
            return await _employeeBasicDetails.GetEmployeeBasicDetailsById(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeBasicDetails( EmployeeBasicDetailsDTO basicDetailsDTO)
        {
            try
            {
                var updatedEmployee = await _employeeBasicDetails.UpdateEmployeeBasicDetails( basicDetailsDTO);
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Updating Basic Employee Details : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        public async Task<String> DeleteEmployeeBasicDetailsById(string id)
        {
            var response = await _employeeBasicDetails.DeleteEmployeeBasicDetailsById(id);
            return response;
        }



/*
        //makepostrequest

        [HttpPost]

        public async Task<IActionResult> AddEmployeeBasicDetailByMakePostRequest(EmployeeBasicDetailsDTO employeeBasicDetailsDTO)
        {
            var response = await _employeeBasicDetails.AddEmployeeBasicDetailByMakePostRequest(employeeBasicDetailsDTO);
            return Ok(response);
        }

        //makegetrequest

        [HttpGet]

        public async Task<List<EmployeeBasicDetailsDTO>> GetEmployeeeBasicDetailByMakeGetRequest()
        {
            var response = await _employeeBasicDetails.GetEmployeeeBasicDetailByMakeGetRequest();
            return response;
        }*/

        //adding visitor by microservises

        [HttpPost]
        public async Task<IActionResult> AddVisitorByMakePostRequest(VisitorDTO visitor)
        {
            var response = await _employeeBasicDetails.AddVisitorByMakePostRequest(visitor);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetVisitorByMakePostRequest()
        {
            var response = await _employeeBasicDetails.GetVisitorByMakePostRequest();
            return Ok(response);
        }
        //MakePost
        [HttpPost]
        public async Task<IActionResult> AddEmployeeBasicDetailsByMakePostRequest(EmployeeBasicDetailsDTO employeeBasicDetailsDto)
        {
            var response = await _employeeBasicDetails.AddEmployeeBasicDetailsByMakePostRequest(employeeBasicDetailsDto);
            return Ok(response);
        }

        [HttpPost]
        [ServiceFilter(typeof(BuildEmployeeFilter))]
        public async Task<EmployeeFilterCriteria> GetAllEmployeesByPagination(EmployeeFilterCriteria employeeFilterCriteria)
        {
            var response = await _employeeBasicDetails.GetAllEmployeesByPagination(employeeFilterCriteria);
            return response;
        }
    }
}
