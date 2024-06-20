using Libaray_Management_System.Data;
using Libaray_Management_System.Interfaces;
using Libaray_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Libaray_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly MemberDBContext _dbContext;
        private readonly IMemberService _memberService;
        public MemberController(MemberDBContext dbContext,IMemberService memberService)
        {
            _dbContext = dbContext;
            _memberService = memberService;
        }
        [HttpPost]
        public async Task<ActionResult<MemberModel>> AddMember(MemberModel memberModel)
        {
            var response = await _memberService.AddMember(memberModel);
            return response;
        }

    }
}
