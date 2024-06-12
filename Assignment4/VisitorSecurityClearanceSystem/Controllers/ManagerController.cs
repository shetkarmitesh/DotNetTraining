using Microsoft.AspNetCore.Mvc;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;
        private readonly IOfficeService _officeService;
        private readonly ISecurityService _securityService;
        private readonly IVisitorService _visitorService;

        public ManagerController(IManagerService managerService, IOfficeService officeService, ISecurityService securityService, IVisitorService visitorService)
        {
            _managerService = managerService;
            _officeService = officeService;
            _securityService = securityService;
            _visitorService = visitorService;

        }

        [HttpPost]
        public async Task<ManagerDTO> AddManager(ManagerDTO managerDTO)
        {
            return await _managerService.AddManager(managerDTO);
        }


        [HttpGet]
        public async Task<ManagerDTO> GetManagerByUId(string uId)
        {
            return await _managerService.GetManagerByUId(uId);
        }

        [HttpPost]
        public async Task<ManagerDTO> UpdateManager(string uId, ManagerDTO managerDTO)
        {

            
                var updatedManager = await _managerService.UpdateManager(uId, managerDTO);
                return updatedManager;
           
        }
        [HttpDelete]
        public async Task<string> DeleteManager(string uId)
        {
            var response = await _managerService.DeleteManager(uId);
            return response;
        }


        [HttpPost]
        public async Task<OfficeDTO> AddOfficeUser(OfficeDTO officeDTO) 
        {
            var office = await _officeService.AddOffice(officeDTO);
            return office;
        }

        [HttpPost]
        public async Task<SecurityDTO> AddSecurityUser(SecurityDTO securityDTO)
        {
            var security = await _securityService.AddSecurity(securityDTO);
            return security;
        }

        [HttpPut]
        public async Task<VisitorDTO> UpdateVisitorStatus(string visitorUId, bool newStatus)
        {
            
                var updatedVisitor = await _visitorService.UpdateVisitorStatus(visitorUId, newStatus);
                return updatedVisitor;
           
        }

        [HttpGet]
        public async Task<List<VisitorDTO>> GetVisitorsByStatus(bool status)
        {
            var visitors = await _visitorService.GetVisitorsByStatus(status);
            return visitors;
        }
        [HttpGet]
        public async Task<List<VisitorDTO>> SearchVisitors(string name = null, string company = null, bool? pass=null,  DateTime? fromDate = null, DateTime? toDate = null)
        {
            // Implement logic to search for visitors based on the provided criteria
            // Use your data access layer (e.g., Entity Framework) to query visitor data
            var searchResults = await _visitorService.SearchVisitors(name, company, fromDate, toDate,pass);
            return searchResults;
        }
    }
}
