using Microsoft.AspNetCore.Mvc;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Controllers
{
    public class VisitorController : Controller
    {
        [ApiController]
        [Route("api/[Controller]/[Action]")]
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
        public async Task<VisitorDTO> GetVisitorByUId(string UId)
        {
            var response = await _visitorService.GetVisitorByUId(UId);
            return response;
        }
        [HttpPost]
        public async Task<VisitorDTO> UpdateVisitor(VisitorDTO visitorDTO)
        {
            var response = await _visitorService.UpdateVisitor(visitorDTO);
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
