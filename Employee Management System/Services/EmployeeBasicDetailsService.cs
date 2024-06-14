using AutoMapper;
using Employee_Management_System.Common;
using Employee_Management_System.CosmosDB;
using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.Services
{
    public class EmployeeBasicDetailsService :IEmployeeBasicDetails
    {
        private readonly ICosmosDBServices _cosmosDBServices;
        private readonly IMapper _mapper;

        public EmployeeBasicDetailsService(ICosmosDBServices cosmosDBServices, IMapper mapper)
        {
            _cosmosDBServices = cosmosDBServices;
            _mapper = mapper;
        }
        public async Task<EmployeeBasicDetailsDTO> AddEmployeeBasicDetails(EmployeeBasicDetailsDTO employeeBasicDetailsDTO)
        {

            var employeeEntity = _mapper.Map<EmployeeBasicDetails>(employeeBasicDetailsDTO);

            employeeEntity.Initialize(true, "employeeBasicDetails", "Admin", "Admin");
            employeeEntity.EmployeeID=employeeEntity.Id;
            var response = await _cosmosDBServices.AddEmployeeBasicDetails(employeeEntity);

            return _mapper.Map<EmployeeBasicDetailsDTO>(response);
        }

        public async Task<List<EmployeeBasicDetailsDTO>> GetAllEmployeeBasicDetails()
        {
            var response = await _cosmosDBServices.GetAllEmployeeBasicDetails();
            var employeeDTOs = new List<EmployeeBasicDetailsDTO>();
            foreach (var employee in response)
            {
                var employeeDTO = _mapper.Map<EmployeeBasicDetailsDTO>(employee);
                employeeDTOs.Add(employeeDTO);
            }
            return employeeDTOs;
        }

        public async Task<EmployeeBasicDetailsDTO> GetEmployeeBasicDetailsById(string id)
        {
            var response = await _cosmosDBServices.GetEmployeeBasicDetailsById(id); 
            return _mapper.Map<EmployeeBasicDetailsDTO>(response);
        }
        /*public async Task<EmployeeBasicDetailsDTO> GetEmployeeBasicDetailsByUId(string uId)
        {
            var response = await _cosmosDBServices.GetEmployeeBasicDetailsByUId(uId); 
            return _mapper.Map<EmployeeBasicDetailsDTO>(response);
        }*/

        public async Task<EmployeeBasicDetailsDTO> UpdateEmployeeBasicDetails( EmployeeBasicDetailsDTO employeeBasicDetailsDTO)
        {
            var employeeEntity = await _cosmosDBServices.GetEmployeeBasicDetailsById(employeeBasicDetailsDTO.EmployeeID);
            if (employeeEntity == null)
            {
                throw new Exception("Employee basic details not found");
            }
            employeeEntity.Active = false;
            employeeEntity.Archived = true;
            await _cosmosDBServices.UpdateEmployeeBasicDetails(employeeEntity);
            _mapper.Map(employeeBasicDetailsDTO, employeeEntity);

            employeeEntity.Initialize(false, "employeeBasicDetails", "Admin", "Admin");
            

            var response = await _cosmosDBServices.AddEmployeeBasicDetails(employeeEntity);
            return _mapper.Map<EmployeeBasicDetailsDTO>(response);
        }

        public async Task<string> DeleteEmployeeBasicDetailsById(string id)
        {
            var employeeToDelete = await _cosmosDBServices.GetEmployeeBasicDetailsById(id);
            employeeToDelete.Active = false;
            employeeToDelete.Archived = true;
            employeeToDelete.Initialize(false, "employeeBasicDetails", "Admin", "Admin");
            await _cosmosDBServices.UpdateEmployeeBasicDetails(employeeToDelete);

            employeeToDelete.Active = false;
            employeeToDelete.Archived = true;

            await _cosmosDBServices.AddEmployeeBasicDetails(employeeToDelete);
            return "Record Deleted Successfully...";
        }
    }
}
