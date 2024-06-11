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
        public async Task<OfficeDTO> AddOffice(OfficeDTO officeModel)
        {

            // Map the DTO to an Entity
            var officeEntity = _mapper.Map<OfficeEntity>(officeModel);

            // Initialize the Entity
            officeEntity.Initialize(true, "office", "Prerit", "Prerit");

            // Add the entity to the database
            var response = await _cosmosDBServices.AddOfficeUser(officeEntity);

            // Map the response back to a DTO
            return _mapper.Map<OfficeDTO>(response);
        }

        public async Task<OfficeDTO> GetOfficeById(string id)
        {
            var office = await _cosmosDBServices.GetOfficeById(id); // Call non-generic method
            return _mapper.Map<OfficeDTO>(office);
        }

        public async Task<OfficeDTO> UpdateOffice(string id, OfficeDTO officeModel)
        {
            var officeEntity = await _cosmosDBServices.GetOfficeById(id);
            if (officeEntity == null)
            {
                throw new Exception("Office not found");
            }
            officeEntity = _mapper.Map<OfficeEntity>(officeModel);
            officeEntity.Id = id;
            var response = await _cosmosDBServices.UpdateOfficeUser(officeEntity);
            return _mapper.Map<OfficeDTO>(response);
        }

        public async Task<string> DeleteOffice(string id)
        {
            /*await _cosmosDBServices.DeleteVisitor(id);*/
            var officeUserToDelete = await _cosmosDBServices.GetManagerById(id);
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
                Id = officeUser.Id,
                Name = officeUser.Name,
                Email = officeUser.Email,
                Phone = officeUser.Phone,
                Role = officeUser.Role,

            };

            return officeDto;
        }
    }
}
