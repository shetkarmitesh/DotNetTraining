using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Entities;

namespace VisitorSecurityClearanceSystem.CosmosDB
{
    public interface ICosmosDBServices
    {
        //Visitors methods
        Task<VisitorEntity> GetVisitorByEmail(string email);
        Task<List<VisitorEntity>> GetVisitorByStatus(bool status);
        Task<VisitorEntity> GetVisitorByUId(string uId);
        Task<List<VisitorEntity>> GetAllVisitors();
        Task<VisitorEntity> AddVisitor(VisitorEntity visitor);
        Task<VisitorEntity> UpdateVisitor(VisitorEntity visitorEntity);


        //Security User methods
        Task<SecurityEntity> GetSecurityByUId(string uId);
        Task<SecurityEntity> GetSecurityUserByEmail(string email);
        Task<SecurityEntity> AddSecurityUser(SecurityEntity security);
        Task<SecurityEntity> UpdateSecurityUser(SecurityEntity securityEntity);





        //Office User methods
        Task<List<OfficeEntity>> GetAllOfficeUser();
        Task<OfficeEntity> GetOfficeByUId(string uId);
        Task<OfficeEntity> GetOfficeUserByEmail(string email);
        Task<OfficeEntity> AddOfficeUser(OfficeEntity officer);
        Task<OfficeEntity> UpdateOfficeUser(OfficeEntity officeUserEntity);

        //Manager User methods
        Task<ManagerEntity> GetManagerByUId(string uId);
        Task<ManagerEntity> AddManager(ManagerEntity manager);
        Task<ManagerEntity> UpdateManager(ManagerEntity managerEntity);

        Task ReplaceAsync(dynamic entity);
    }

}
