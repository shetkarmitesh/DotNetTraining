using AutoMapper;
using Employee_Management_System.Common;
using Employee_Management_System.CosmosDB;
using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.Services
{
    public class EmployeeAdditionalDetailsService:IEmployeeAdditionalDetails
    {
       
            private readonly ICosmosDBServices _cosmosDBServices;
            private readonly IMapper _mapper;
            private readonly IEmployeeBasicDetails _employeeBasicDetailsService;

        public EmployeeAdditionalDetailsService(ICosmosDBServices cosmosDBServices, IMapper mapper,IEmployeeBasicDetails employeeBasicDetailsService)
            {
                _cosmosDBServices = cosmosDBServices;
                _mapper = mapper;
            _employeeBasicDetailsService = employeeBasicDetailsService;
            }
            public async Task<EmployeeAdditionalDetailsDTO> AddEmployeeAdditionalDetails(EmployeeAdditionalDetailsDTO employeeAdditionalDetailsDTO)
            {
            var employeeBasicDetails = await _employeeBasicDetailsService.GetEmployeeBasicDetailsByUId(employeeAdditionalDetailsDTO.EmployeeBasicDetailsUId);
                 if (employeeBasicDetails == null)
                {
                    throw new Exception("Employee Basic details not found ");
                }

            var employeeEntity = _mapper.Map<EmployeeAdditionalDetails>(employeeAdditionalDetailsDTO);
            employeeEntity.Initialize(true, "employeeAdditionalDetails", "Admin", "Admin");

            var response = await _cosmosDBServices.AddEmployeeAdditionalDetails(employeeEntity);

                return _mapper.Map<EmployeeAdditionalDetailsDTO>(response);
            }

        public async Task<List<EmployeeAdditionalDetailsDTO>> GetAllEmployeeAdditionalDetails()
        {
            var response = await _cosmosDBServices.GetAllEmployeeAdditionalDetails();
            var employeeDTOs = new List<EmployeeAdditionalDetailsDTO>();
            foreach (var employee in response)
            {
                var employeeDTO = _mapper.Map<EmployeeAdditionalDetailsDTO>(employee);
                employeeDTOs.Add(employeeDTO);
            }
            return employeeDTOs;
        }
            public async Task<EmployeeAdditionalDetailsDTO> GetAllEmployeeAdditionalDetailsById(string id)
            {
                var rsponse = await _cosmosDBServices.GetAllEmployeeAdditionalDetailsById(id); 
                return _mapper.Map<EmployeeAdditionalDetailsDTO>(rsponse);
            }

            public async Task<EmployeeAdditionalDetailsDTO> UpdateEmployeeAdditionalDetails( EmployeeAdditionalDetailsDTO employeeAdditionalDetailsDTO)
            {
                var employeeEntity = await _cosmosDBServices.GetAllEmployeeAdditionalDetailsById(employeeAdditionalDetailsDTO.EmployeeBasicDetailsUId);//id
                if (employeeEntity == null)
                {
                    throw new Exception("Employee additional details not found");
                }
                employeeEntity.Active = false;
                employeeEntity.Archived = true;
                await _cosmosDBServices.UpdateEmployeeAdditionalDetails(employeeEntity);
                 _mapper.Map(employeeAdditionalDetailsDTO, employeeEntity);
           
                employeeEntity.Initialize(false, "employeeAdditionalDetails", "Admin", "Admin");
            
            var response = await _cosmosDBServices.AddEmployeeAdditionalDetails(employeeEntity);
                return _mapper.Map<EmployeeAdditionalDetailsDTO>(response);
        }

            public async Task<string> DeleteEmployeeAdditionalDetailsById(string id)
            {
                var employeeToDelete = await _cosmosDBServices.GetAllEmployeeAdditionalDetailsById(id);
                employeeToDelete.Active = false;
                employeeToDelete.Archived = true;
                await _cosmosDBServices.UpdateEmployeeAdditionalDetails(employeeToDelete);

                employeeToDelete.Initialize(false, "employeeAdditionalDetails", "Admin", "Admin");
                employeeToDelete.Active = false;
                employeeToDelete.Archived = true;

            await _cosmosDBServices.AddEmployeeAdditionalDetails(employeeToDelete);
            return "Record Deleted Successfully...";
            }
    }
}
