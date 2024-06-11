using VisitorSecurityClearanceSystem.DTOs;

namespace VisitorSecurityClearanceSystem.Interfaces
{
    public interface IOfficeService
    {
        Task<OfficeDTO> AddOffice(OfficeDTO officeDTO);
        Task<OfficeDTO> GetOfficeByUId(string uId);
        Task<OfficeDTO> UpdateOffice(string uId, OfficeDTO officeDTO);
        Task<string> DeleteOffice(string uId);

        Task<OfficeDTO> LoginOfficeUser(string email, string password);
    }
}
