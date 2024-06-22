using AutoMapper;
using Libaray_Management_System.Entities;
using Libaray_Management_System.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Libaray_Management_System.Common
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile() {
            CreateMap<MemberEntry, MemberModel>().ReverseMap();
            CreateMap<BookEntity, BookModel>().ReverseMap();
            CreateMap<IssueBookEntity, IssueBookModel>().ReverseMap();
        }
    }
}
