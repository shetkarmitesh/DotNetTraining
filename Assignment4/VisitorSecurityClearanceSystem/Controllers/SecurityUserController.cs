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
        public async Task<IActionResult> Login(LoginDTO loginModel)
        {
            var securityUser = await _securityService.LoginSecurityUser(loginModel.Email, loginModel.Password);
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
        public async Task<VisitorDTO> GetVisitorById(string id)
        {
            return await _visitorService.GetVisitorById(id);
        }



        [HttpGet]
        public async Task<SecurityDTO> GetSecurityById(string id)
        {
            return await _securityService.GetSecurityById(id);
        }

        [HttpPost]
        public async Task<SecurityDTO> UpdateSecurity(string id, SecurityDTO securityModel)
        {
            
                var updatedSecurity = await _securityService.UpdateSecurity(id, securityModel);
                return updatedSecurity;
           
        }

        [HttpDelete]
        public async Task<string> DeleteSecurity(string id)
        {
            var response = await _visitorService.DeleteVisitor(id);
            return response;
        }
    }
}
