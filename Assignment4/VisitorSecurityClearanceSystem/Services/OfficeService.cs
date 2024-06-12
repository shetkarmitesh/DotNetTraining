using AutoMapper;
using VisitorSecurityClearanceSystem.Common;
using VisitorSecurityClearanceSystem.CosmosDB;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Entities;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly ICosmosDBServices _cosmosDBServices;
        private readonly IMapper _mapper;

        public OfficeService(ICosmosDBServices cosmosDBServices, IMapper mapper)
        {
            _cosmosDBServices = cosmosDBServices;
            _mapper = mapper;
        }
        public async Task<OfficeDTO> AddOffice(OfficeDTO officeDTO)
        {

            // Map the DTO to an Entity
            var officeEntity = _mapper.Map<OfficeEntity>(officeDTO);

            // Initialize the Entity
            officeEntity.Initialize(true, "office", "Admin", "Admin");

            // Add the entity to the database
            var response = await _cosmosDBServices.AddOfficeUser(officeEntity);

            // Map the response back to a DTO
            return _mapper.Map<OfficeDTO>(response);
        }
        public async Task<List<OfficeDTO>> GetAllOfficeUser()
        {
            var officeUsers = await _cosmosDBServices.GetAllOfficeUser();
            var officeDTOs = new List<OfficeDTO>();
            foreach (var office in officeUsers)
            {
                var officeDTO = _mapper.Map<OfficeDTO>(office);
                officeDTOs.Add(officeDTO);
            }
            return officeDTOs;
        }

        public async Task<OfficeDTO> GetOfficeByUId(string uId)
        {
            var office = await _cosmosDBServices.GetOfficeByUId(uId);
            return _mapper.Map<OfficeDTO>(office);
        }

        public async Task<OfficeDTO> UpdateOffice(string uId, OfficeDTO officeDTO)
        {
            var officeEntity = await _cosmosDBServices.GetOfficeByUId(uId);
            if (officeEntity == null)
            {
                throw new Exception("OfficeUser not found");
            }
            /*officeEntity = _mapper.Map<OfficeEntity>(officeDTO);*/
            officeEntity.Active = false;
            officeEntity.Archived = true;
            await _cosmosDBServices.ReplaceAsync(officeEntity);

            officeEntity.Initialize(false, "office", "Admin", "Admin");
            officeEntity.UId = officeDTO.UId;
            officeEntity.Name = officeDTO.Name;
            officeEntity.Email = officeDTO.Email;
            officeEntity.Phone = officeDTO.Phone;
            officeEntity.Role = officeDTO.Role;
            officeEntity.CompanyName = officeDTO.CompanyName;
            
            var response = await _cosmosDBServices.UpdateOfficeUser(officeEntity);
            return _mapper.Map<OfficeDTO>(response);
        }

        public async Task<string> DeleteOffice(string uId)
        {
            /*await _cosmosDBServices.DeleteVisitor(id);*/
            var officeUserToDelete = await _cosmosDBServices.GetManagerByUId(uId);
            officeUserToDelete.Active = false;
            officeUserToDelete.Archived = true;
            await _cosmosDBServices.UpdateManager(officeUserToDelete);

            officeUserToDelete.Initialize(false, Credentials.VisitorDocumnetType, "Admin", "Admin");
            officeUserToDelete.Active = false;
            officeUserToDelete.Archived = true;

            await _cosmosDBServices.AddManager(officeUserToDelete);
            return "Record Deleted Successfully...";
        }


        public async Task<OfficeDTO> LoginOfficeUser(string email, string password)
        {
            // Fetch the manager entity by email
            var officeUser = await _cosmosDBServices.GetOfficeUserByEmail(email);

            if (officeUser == null || officeUser.Password != password)
            {
                return null; // Credentials are invalid
            }

            // Map ManagerEntity to ManagerDTO
            var officeDto = new OfficeDTO
            {
                UId = officeUser.UId,
                Name = officeUser.Name,
                Email = officeUser.Email,
                Phone = officeUser.Phone,
                Role = officeUser.Role,

            };

            return officeDto;
        }
    }
}
