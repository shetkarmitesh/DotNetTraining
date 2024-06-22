using AutoMapper.Execution;
using Libaray_Management_System.Data;
using Libaray_Management_System.Interfaces;
using Libaray_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libaray_Management_System.Controllers
{
    [Route("api/[Controller]/[Action]")]
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

        [HttpGet]
         public async Task<IEnumerable<MemberModel>> GetAllMembers()
        {
            return await _memberService.GetAllMembers();
        }

        [HttpGet]
         public async Task<MemberModel> GetMemberByUId(int uId)
        {
            return await _memberService.GetMemberByMemberId(uId);
        }

        [HttpPost]
        public async Task<ActionResult<MemberModel>> AddMember(MemberModel memberModel)
        {
            var response = await _memberService.AddMember(memberModel);
            return response;
        }
        [HttpPut]
        public async Task<ActionResult<MemberModel>> UpdateMember(MemberModel memberModel)
        {
            var response = await _memberService.UpdateMember(memberModel);
            return response;
        }

        [HttpDelete]
        public async Task<MemberModel> DeleteMember(int uId)
        {
            var response = await _memberService.DeleteMember(uId);
            return response;
        }

    }
}
