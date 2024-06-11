using VisitorSecurityClearanceSystem.DTOs;

namespace VisitorSecurityClearanceSystem.Interfaces
{
    public interface IManagerService
    {
        Task<ManagerDTO> AddManager(ManagerDTO managerDTO);
        Task<ManagerDTO> GetManagerByUId(string uId);
        Task<ManagerDTO> UpdateManager(string uId, ManagerDTO managerDTO);
        Task<string> DeleteManager(string uId);
    }
}
