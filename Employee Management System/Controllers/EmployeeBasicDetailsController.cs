﻿using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    }
}
