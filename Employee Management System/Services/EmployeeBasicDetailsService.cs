using AutoMapper;
using Employee_Management_System.Common;
using Employee_Management_System.CosmosDB;
using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VisitorSecurityClearanceSystem.DTOs;

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
       

        public async Task<VisitorDTO> AddVisitorByMakePostRequest(VisitorDTO visitor)
        {
            var serialObj = JsonConvert.SerializeObject(visitor);
            var requestObj = await HttpClientHelper.MakePostRequest(Credentials.VisitorUrl, Credentials.AddVisitorEndPoint, serialObj);
            var responseObj = JsonConvert.DeserializeObject<VisitorDTO>(requestObj);
            return responseObj;

        }


        public async Task<IEnumerable<VisitorDTO>> GetVisitorByMakePostRequest()
        {
            var responseString = await HttpClientHelper.MakeGetRequest(Credentials.VisitorUrl, Credentials.GetAllVisitorEndPoint);
            var employees = JsonConvert.DeserializeObject<IEnumerable<VisitorDTO>>(responseString);
            return employees;
        }


        public async Task<EmployeeBasicDetailsDTO> AddEmployeeBasicDetailsByMakePostRequest(EmployeeBasicDetailsDTO employeeBasicDetailsDto)
        {
            var serialObj = JsonConvert.SerializeObject(employeeBasicDetailsDto);
            var requestObj = await HttpClientHelper.MakePostRequest(Credentials.EmployeeUrl, Credentials.AddEmployeeEndPoint, serialObj);
            var responseObj = JsonConvert.DeserializeObject<EmployeeBasicDetailsDTO>(requestObj);
            return responseObj;

        }
        public async Task<IEnumerable<EmployeeBasicDetailsDTO>> GetEmployeeBasicDetailsByMakeGetRequest()
        {
            var responseString = await HttpClientHelper.MakeGetRequest(Credentials.EmployeeUrl, Credentials.GetAllEmployeesEndPoint);
            var employees = JsonConvert.DeserializeObject<IEnumerable<EmployeeBasicDetailsDTO>>(responseString);
            return employees;
        }

        public async Task<EmployeeFilterCriteria> GetAllEmployeesByPagination(EmployeeFilterCriteria employeeFilterCriteria)
        {
            EmployeeFilterCriteria response = new EmployeeFilterCriteria();

            var checkFilter = employeeFilterCriteria.Filters.Any(x => x.fieldName == "role");
            var role = "";
            if (checkFilter)
            {
                role = employeeFilterCriteria.Filters.Find(a => a.fieldName == "role").fieldValue.FirstOrDefault();
            }

            var employees = await GetAllEmployeeBasicDetails();

            var filterRecords = string.IsNullOrEmpty(role) ? employees : employees.Where(a => a.Role == role);

            var employeeList = filterRecords.ToList();

            response.TotalCount = employeeList.Count;

            response.page = employeeFilterCriteria.page;
            response.pageSize = employeeFilterCriteria.pageSize;

            if (response.page < 1) response.page = 1;
            if (response.pageSize < 1) response.pageSize = 10;
            var skip = response.pageSize * (response.page - 1);

            var pagedRecords = employeeList.Skip(skip).Take(response.pageSize).ToList();

            response.Employees = pagedRecords;

            return response;
        }

        public async Task<List<EmployeeBasicDetailsDTO>> GetAllEmployeeBasicDetailsByRole(string role)
        {
            var allEmployees = await GetAllEmployeeBasicDetails();
            return allEmployees.FindAll(e => e.Role == role);
        }
    }
}
