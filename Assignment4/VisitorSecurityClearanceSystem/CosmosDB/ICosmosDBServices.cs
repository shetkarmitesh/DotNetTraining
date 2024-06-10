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
        /*Task<string> DeleteVisitor(string id);*/
    }
}
