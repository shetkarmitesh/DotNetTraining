using VisitorSecurityClearanceSystem.DTOs;

namespace VisitorSecurityClearanceSystem.Interfaces
{
    public interface IManagerService
    {
        Task<ManagerDTO> AddManager(ManagerDTO managerDTO);
        Task<ManagerDTO> GetManagerById(string id);
        Task<ManagerDTO> UpdateManager(string id, ManagerDTO managerDTO);
        Task<string> DeleteManager(string id);
    }
}
