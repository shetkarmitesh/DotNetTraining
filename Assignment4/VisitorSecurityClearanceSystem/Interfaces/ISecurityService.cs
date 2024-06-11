using VisitorSecurityClearanceSystem.DTOs;

namespace VisitorSecurityClearanceSystem.Interfaces
{
    public interface ISecurityService
    {
        Task<SecurityDTO> AddSecurity(SecurityDTO securityDTO);
        Task<SecurityDTO> GetSecurityById(string id);
        Task<SecurityDTO> UpdateSecurity(string id, SecurityDTO securityDTO);
        Task<string> DeleteSecurity(string id);

        Task<SecurityDTO> LoginSecurityUser(string email, string password);
    }
}
