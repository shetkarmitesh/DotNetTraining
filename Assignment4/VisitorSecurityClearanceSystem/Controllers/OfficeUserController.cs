using Microsoft.AspNetCore.Mvc;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Interfaces;
using VisitorSecurityClearanceSystem.Services;

namespace VisitorSecurityClearanceSystem.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class OfficeUserController : Controller
    {

        private readonly IOfficeService _officeService;
        private readonly IVisitorService _visitorService;


        public OfficeUserController(IOfficeService officeService)
        {
            _officeService = officeService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var officeUser = await _officeService.LoginOfficeUser(loginDTO.Email, loginDTO.Password);
            if (officeUser == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(officeUser);
        }

        [HttpGet]
        public async Task<OfficeDTO> GetOfficeByUId(string uId)
        {
            return await _officeService.GetOfficeByUId(uId);
        }[HttpGet]
        public async Task<List<OfficeDTO>> GetAllOfficeUsers()
        {
            return await _officeService.GetAllOfficeUser();
        }

        [HttpPut]
        public async Task<OfficeDTO> UpdateOffice(string uId, OfficeDTO officeDTO)
        {
            
                var updatedSecurity = await _officeService.UpdateOffice(uId, officeDTO);
                return updatedSecurity;
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

        [HttpDelete]
        public async Task<string> DeleteOffice(string uId)
        {  
            var response = await _officeService.DeleteOffice(uId);
            return response;
        }
        
    }
}
