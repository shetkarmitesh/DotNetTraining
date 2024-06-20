using AutoMapper.Execution;
using Libaray_Management_System.Data;
using Libaray_Management_System.Entities;
using Libaray_Management_System.Interfaces;
using Libaray_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libaray_Management_System.Services
{
    public class MemberService : IMemberService
    {
        private readonly MemberDBContext _memberDBContext;

        public MemberService(MemberDBContext memberDBContext)
        {
            _memberDBContext = memberDBContext; 
        }
        
        public async Task<ActionResult<MemberModel>> AddMember(MemberModel memberModel)
        {
            var existingMenmber = await GetMemberByEmail(memberModel.Email);
            if (existingMenmber != null)
            {
                throw new InvalidOperationException("A member already exists with this email.");
            }
            MemberEntity memberEntity = new MemberEntity();
            memberEntity.FirstName = memberModel.FirstName; 
            memberEntity.LastName = memberModel.LastName; 
            memberEntity.Email = memberModel.Email;
            memberEntity.Initialize(true, "Admin");
            //use mapper
             _memberDBContext.MemberEntity.Add(memberEntity);
            _memberDBContext.SaveChanges();

            return memberModel;

            // OR: Option 2: Map memberEntity to MemberModel if necessary
            // return Ok(Mapper.Map<MemberModel>(memberEntity));
        }

        public async Task<IEnumerable<MemberModel>> GetAllMembers()
        {
            var response =  await _memberDBContext.MemberEntity.Where(q => q.Active && !q.Archived).ToListAsync();
            var memberModels = new List<MemberModel>();
            foreach (var member in response)
            {
                /*var visitorDTO = _mapper.Map<VisitorDTO>(visitor);*/
                MemberModel memberModel = new MemberModel();
                memberModel.FirstName = member.FirstName;
                memberModel.LastName = member.LastName;
                memberModel.Email = member.Email;   
                memberModel.PhoneNo = member.PhoneNo;
                memberModel.UId = member.UId;
                memberModels.Add(memberModel);
            }
            return memberModels;
        }

        public async Task<MemberModel> GetMemberByUId(int uId)
        {
            var member = await _memberDBContext.MemberEntity.Where(q => q.UId==uId && q.Active && !q.Archived ).FirstOrDefaultAsync();
            MemberModel memberModel = new MemberModel();
            memberModel.FirstName = member.FirstName;
            memberModel.LastName = member.LastName;
            memberModel.Email = member.Email;
            memberModel.PhoneNo = member.PhoneNo;
            memberModel.UId = member.UId;
            return memberModel;
        }
        public async Task<MemberModel> GetMemberByEmail(string email)
        {
            var member = await _memberDBContext.MemberEntity.Where(q => q.Email == email && q.Active && !q.Archived).FirstOrDefaultAsync();
            MemberModel memberModel = new MemberModel();
            memberModel.FirstName = member.FirstName;
            memberModel.LastName = member.LastName;
            memberModel.Email = member.Email;
            memberModel.PhoneNo = member.PhoneNo;
            memberModel.UId = member.UId;
            return memberModel;
        }
        public async Task<ActionResult<MemberModel>> UpdateMember(MemberModel memberModel)
        {
            var existingMenmber = await _memberDBContext.MemberEntity.Where(q => q.UId == memberModel.UId && q.Active && !q.Archived).FirstOrDefaultAsync();
            if (existingMenmber != null)
            {
                throw new InvalidOperationException("A member not found.");
            }
            existingMenmber.Active = false;
            existingMenmber.Archived = true;
            await _memberDBContext.SaveChangesAsync();

            existingMenmber.Initialize(false, "Admin");
            existingMenmber.FirstName = memberModel.FirstName;
            existingMenmber.LastName = memberModel.LastName;
            existingMenmber.Email = memberModel.Email;
            existingMenmber.PhoneNo= memberModel.PhoneNo;

            _memberDBContext.MemberEntity.Add(existingMenmber);
            _memberDBContext.SaveChanges();
            return (memberModel);
        }
        public async Task<IActionResult> DeleteMember(int uId)
        {
            var memberToDelete = await _memberDBContext.MemberEntity.Where(q => q.UId == uId && q.Active && !q.Archived).FirstOrDefaultAsync();
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
            return NoContent();
        }
    }
}
