using VisitorSecurityClearanceSystem.DTOs;

namespace VisitorSecurityClearanceSystem.Interfaces
{
    public interface IVisitorService
    {
        Task<VisitorDTO> AddVisitor(VisitorDTO visitor);
        Task<List<VisitorDTO>> GetAllVisitors();
        Task<VisitorDTO> GetVisitorByUId(string uId);
        Task<VisitorDTO> UpdateVisitor(string uId, VisitorDTO visitorDTO);
        Task<VisitorDTO> UpdateVisitorStatus(string visitorId, bool newStatus);
        Task<List<VisitorDTO>> GetVisitorsByStatus(bool status);
        Task<string> DeleteVisitor(string uId);
    }
}
