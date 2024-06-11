using Microsoft.AspNetCore.Mvc;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Interfaces;
using VisitorSecurityClearanceSystem.Services;

namespace VisitorSecurityClearanceSystem.Controllers
{
    public class OfficeUserController : Controller
    {

        private readonly IOfficeService _officeService;
        private readonly IVisitorService _visitorService;


        public OfficeUserController(IOfficeService officeService)
        {
            _officeService = officeService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginModel)
        {
            var officeUser = await _officeService.LoginOfficeUser(loginModel.Email, loginModel.Password);
            if (officeUser == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(officeUser);
        }

        [HttpGet]
        public async Task<OfficeDTO> GetOfficeById(string id)
        {
            return await _officeService.GetOfficeById(id);
        }

        [HttpPut]
        public async Task<OfficeDTO> UpdateOffice(string id, OfficeDTO securityModel)
        {
            
                var updatedSecurity = await _officeService.UpdateOffice(id, securityModel);
                return updatedSecurity;
           
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


        [HttpDelete]
        public async Task<string> DeleteOffice(string id)
        {
            
            var response = await _officeService.DeleteOffice(id);
            return response;
        }
        
    }
}
