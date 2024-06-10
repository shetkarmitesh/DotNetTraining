using AutoMapper;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Entities;

namespace VisitorSecurityClearanceSystem.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VisitorEntity, VisitorDTO>().ReverseMap();
            CreateMap<SecurityEntity, SecurityDTO>().ReverseMap();
            CreateMap<OfficeEntity, OfficeDTO>().ReverseMap();
            CreateMap<ManagerEntity, ManagerDTO>().ReverseMap();
        }
    }

   
}
