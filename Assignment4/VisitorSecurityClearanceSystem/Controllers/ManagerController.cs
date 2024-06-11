using Microsoft.AspNetCore.Mvc;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Controllers
{
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
        public async Task<ManagerDTO> GetManagerById(string id)
        {
            return await _managerService.GetManagerById(id);
        }

        [HttpPost]
        public async Task<ManagerDTO> UpdateManager(string id, ManagerDTO managerDTO)
        {

            
                var updatedManager = await _managerService.UpdateManager(id, managerDTO);
                return updatedManager;
           
        }
        [HttpDelete]
        public async Task<string> DeleteManager(string id)
        {
            var response = await _managerService.DeleteManager(id);
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
        public async Task<VisitorDTO> UpdateVisitorStatus(string visitorId, bool newStatus)
        {
            
                var updatedVisitor = await _visitorService.UpdateVisitorStatus(visitorId, newStatus);
                return updatedVisitor;
           
        }

        [HttpGet]
        public async Task<List<VisitorDTO>> GetVisitorsByStatus(bool status)
        {
            var visitors = await _visitorService.GetVisitorsByStatus(status);
            return visitors;
        }
        
    }
}
