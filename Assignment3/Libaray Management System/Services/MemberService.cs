using AutoMapper;
using AutoMapper.Execution;
using Libaray_Management_System.Data;
using Libaray_Management_System.Entities;
using Libaray_Management_System.Interfaces;
using Libaray_Management_System.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libaray_Management_System.Services
{
    public class MemberService : IMemberService
    {
        private readonly MemberDBContext _memberDBContext;
        private readonly IMapper _mapper;

        public MemberService(MemberDBContext memberDBContext,IMapper mapper)
        {
            _memberDBContext = memberDBContext;
            _mapper = mapper;

        }

        public async Task<ActionResult<MemberModel>> AddMember(MemberModel memberModel)
        {
            var existingMenmber = await GetMemberByEmail(memberModel.Email);
            if (existingMenmber != null)
            {
                throw new InvalidOperationException("A member already exists with this email.");
            }
            var member = _mapper.Map<MemberEntity>(memberModel);
       
            member.Initialize(true, "Admin");
             _memberDBContext.MemberEntity.Add(member);
            var response = _memberDBContext.SaveChanges();

            return _mapper.Map<MemberModel>(response);

            
        }

        public async Task<IEnumerable<MemberModel>> GetAllMembers()
        {
            var response =  await _memberDBContext.MemberEntity.Where(q => q.Active && !q.Archived).ToListAsync();
            var memberModels = new List<MemberModel>();
            foreach (var member in response)
            {
                MemberModel memberModel = new MemberModel();
             
                memberModel = _mapper.Map<MemberModel>(member);
                memberModels.Add(memberModel);
            }
            return memberModels;
        }

        public async Task<MemberModel> GetMemberByMemberId(int memberId)
        {
            var member = await _memberDBContext.MemberEntity.Where(q => q.MemberId==memberId && q.Active && !q.Archived ).FirstOrDefaultAsync();
            if (member == null)
            {
                return null;
            }
           
            return _mapper.Map<MemberModel>(member);
        }
        public async Task<MemberModel> GetMemberByEmail(string email)
        {
            var member = await _memberDBContext.MemberEntity.Where(q => q.Email == email && q.Active && !q.Archived).FirstOrDefaultAsync();
            if (member == null)
            {
                return null;
            }
            /*MemberModel memberModel = new MemberModel();*/

            return _mapper.Map<MemberModel>(member);
        }
        public async Task<ActionResult<MemberModel>> UpdateMember(MemberModel memberModel)
        {
            var existingMenmber = await _memberDBContext.MemberEntity.Where(q => q.MemberId == memberModel.MemberId && q.Active && !q.Archived).FirstOrDefaultAsync();
            if (existingMenmber != null)
            {
                throw new InvalidOperationException("A member not found.");
            }
            existingMenmber.Active = false;
            existingMenmber.Archived = true;
            await _memberDBContext.SaveChangesAsync();

            existingMenmber.Initialize(false, "Admin");
            /*existingMenmber.FirstName = memberModel.FirstName;
            existingMenmber.LastName = memberModel.LastName;
            existingMenmber.Email = memberModel.Email;
            existingMenmber.PhoneNo= memberModel.PhoneNo;*/
            existingMenmber = _mapper.Map<MemberEntity>(memberModel);

            _memberDBContext.MemberEntity.Add(existingMenmber);
            _memberDBContext.SaveChanges();
            return (memberModel);
        }
        public async Task<MemberModel> DeleteMember(int memberId)
        {
            var memberToDelete = await _memberDBContext.MemberEntity.Where(q => q.MemberId == memberId && q.Active && !q.Archived).FirstOrDefaultAsync();

            if (memberToDelete != null)
            {
                throw new InvalidOperationException("A member not found.");
            }
            memberToDelete.Active = false;
            memberToDelete.Archived = true;
            await _memberDBContext.SaveChangesAsync();
            memberToDelete.Initialize(false, "Admin");
            memberToDelete.Active = false;
            memberToDelete.Archived = true;
            _memberDBContext.MemberEntity.Add(memberToDelete);
            _memberDBContext.SaveChanges();

            return _mapper.Map<MemberModel>(memberToDelete);
        }
    }
}
