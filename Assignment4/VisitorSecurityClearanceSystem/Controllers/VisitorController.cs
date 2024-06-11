using Microsoft.AspNetCore.Mvc;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class VisitorController : Controller
    {
        
        private readonly IVisitorService _visitorService;
        public VisitorController(IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        [HttpPost]
        public async Task<VisitorDTO> AddVisitor(VisitorDTO visitorDTO)
        {
            var response = await _visitorService.AddVisitor(visitorDTO);
            return response;
        }
        [HttpGet]
        public async Task<List<VisitorDTO>> GetAllVisitors()
        {
            var response = await _visitorService.GetAllVisitors();
            return response;
        }

        [HttpGet]
        public async Task<VisitorDTO> GetVisitorById(string UId)
        {
            var response = await _visitorService.GetVisitorById(UId);
            return response;
        }
        [HttpPost]
        public async Task<VisitorDTO> UpdateVisitor(string id, VisitorDTO visitorDTO)
        {
            var response = await _visitorService.UpdateVisitor(id, visitorDTO);
            return response;
        }
        [HttpDelete]
        public async Task<string> DeleteVisitor(string UId)
        {
            var response = await _visitorService.DeleteVisitor(UId);
            return response;
        }

    }

}
