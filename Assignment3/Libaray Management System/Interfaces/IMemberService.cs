using Libaray_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Libaray_Management_System.Interfaces
{
    public interface IMemberService
    {
        Task<ActionResult<MemberModel>> AddMember(MemberModel memberModel);
        Task<MemberModel> GetMemberByUId(int uId);
        Task<MemberModel> GetMemberByEmail(string email);
    }
}
