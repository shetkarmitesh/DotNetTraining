using VisitorSecurityClearanceSystem.DTOs;

namespace VisitorSecurityClearanceSystem.Interfaces
{
    public interface ISecurityService
    {
        Task<SecurityDTO> AddSecurity(SecurityDTO securityDTO);
        Task<SecurityDTO> GetSecurityByUId(string uId);
        Task<SecurityDTO> UpdateSecurity(string uId, SecurityDTO securityDTO);
        Task<string> DeleteSecurity(string uId);

        Task<SecurityDTO> LoginSecurityUser(string email, string password);
    }
}
