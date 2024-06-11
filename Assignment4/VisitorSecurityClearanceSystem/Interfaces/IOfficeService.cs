using VisitorSecurityClearanceSystem.DTOs;

namespace VisitorSecurityClearanceSystem.Interfaces
{
    public interface IOfficeService
    {
        Task<OfficeDTO> AddOffice(OfficeDTO officeDTO);
        Task<OfficeDTO> GetOfficeById(string id);
        Task<OfficeDTO> UpdateOffice(string id, OfficeDTO officeDTO);
        Task<string> DeleteOffice(string id);

        Task<OfficeDTO> LoginOfficeUser(string email, string password);
    }
}
