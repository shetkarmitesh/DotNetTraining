using VisitorSecurityClearanceSystem.DTOs;

namespace VisitorSecurityClearanceSystem.Interfaces
{
    public interface IVisitorService
    {
        Task<VisitorDTO> AddVisitor(VisitorDTO visitor);
        Task<List<VisitorDTO>> GetAllVisitors();
        Task<VisitorDTO> GetVisitorByUId(string id);
        Task<VisitorDTO> UpdateVisitor(string id, VisitorDTO visitorModel);
        Task<VisitorDTO> UpdateVisitorStatus(string visitorId, bool newStatus);
        Task<List<VisitorDTO>> GetVisitorsByStatus(bool status);
        Task<string> DeleteVisitor(string id);
    }
}
