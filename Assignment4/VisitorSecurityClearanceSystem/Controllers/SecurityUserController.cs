using Microsoft.AspNetCore.Mvc;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class SecurityUserController : Controller
    {
        private readonly ISecurityService _securityService;
        private readonly IVisitorService _visitorService;
        public SecurityUserController(ISecurityService securityService, IVisitorService visitorService)
        {
            _securityService = securityService;
            _visitorService = visitorService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var securityUser = await _securityService.LoginSecurityUser(loginDTO.Email, loginDTO.Password);
            if (securityUser == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(securityUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetVisitorsByStatus(bool status)
        {
            var visitors = await _visitorService.GetVisitorsByStatus(status);
            return Ok(visitors);
        }

        [HttpGet]
        public async Task<VisitorDTO> GetVisitorByUId(string uId)
        {
            return await _visitorService.GetVisitorByUId(uId);
        }



        [HttpGet]
        public async Task<SecurityDTO> GetSecurityByUId(string uId)
        {
            return await _securityService.GetSecurityByUId(uId);
        }

        [HttpPost]
        public async Task<SecurityDTO> UpdateSecurity(string uId, SecurityDTO securityDTO)
        {
            
                var updatedSecurity = await _securityService.UpdateSecurity(uId, securityDTO);
                return updatedSecurity;
           
        }

        [HttpDelete]
        public async Task<string> DeleteSecurity(string uId)
        {
            var response = await _visitorService.DeleteVisitor(uId);
            return response;
        }
    }
}
