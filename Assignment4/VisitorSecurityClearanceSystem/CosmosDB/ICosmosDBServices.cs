using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Entities;

namespace VisitorSecurityClearanceSystem.CosmosDB
{
    public interface ICosmosDBServices
    {
        //Visitors methods
        Task<VisitorEntity> GetVisitorByEmail(string email);
        Task<List<VisitorEntity>> GetVisitorByStatus(bool status);
        Task<VisitorEntity> GetVisitorById(string id);
        Task<List<VisitorEntity>> GetAllVisitors();
        Task<VisitorEntity> AddVisitor(VisitorEntity visitor);
        Task<VisitorEntity> UpdateVisitor(VisitorEntity visitorEntity);


        //Security User methods
        Task<SecurityEntity> GetSecurityById(string id);
        Task<SecurityEntity> GetSecurityUserByEmail(string email);
        Task<SecurityEntity> AddSecurityUser(SecurityEntity security);
        Task<SecurityEntity> UpdateSecurityUser(SecurityEntity securityEntity);
        




        //Office User methods
        Task<OfficeEntity> GetOfficeById(string id);
        Task<OfficeEntity> GetOfficeUserByEmail(string email);
        Task<OfficeEntity> AddOfficeUser(OfficeEntity officer);
        Task<OfficeEntity> UpdateOfficeUser(OfficeEntity officeUserEntity);

        //Manager User methods
        Task<ManagerEntity> GetManagerById(string id);
        Task<ManagerEntity> AddManager(ManagerEntity manager);
        Task<ManagerEntity> UpdateManager(ManagerEntity managerEntity);


    }
}
