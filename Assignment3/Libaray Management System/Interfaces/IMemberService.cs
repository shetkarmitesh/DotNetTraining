using Libaray_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Libaray_Management_System.Interfaces
{
    public interface IMemberService
    {
        Task<ActionResult<MemberModel>> AddMember(MemberModel memberModel);
        Task<IEnumerable<MemberModel>> GetAllMembers();
        Task<MemberModel> GetMemberByMemberId(int memberI);
        Task<MemberModel> GetMemberByEmail(string email);
        Task<ActionResult<MemberModel>> UpdateMember(MemberModel memberModel);
        Task<MemberModel> DeleteMember(int memberId);
    }
}
