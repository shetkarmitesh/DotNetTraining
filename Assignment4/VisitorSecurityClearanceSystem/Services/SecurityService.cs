using AutoMapper;
using VisitorSecurityClearanceSystem.Common;
using VisitorSecurityClearanceSystem.CosmosDB;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Entities;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ICosmosDBServices _cosmosDBServices;
        private readonly IMapper _mapper;

        public SecurityService(ICosmosDBServices cosmosDBServices, IMapper mapper)
        {
            _cosmosDBServices = cosmosDBServices;
            _mapper = mapper;
        }
        public async Task<SecurityDTO> AddSecurity(SecurityDTO securityModel)
        {

            // Map the DTO to an Entity
            var securityEntity = _mapper.Map<SecurityEntity>(securityModel);

            // Initialize the Entity
            securityEntity.Initialize(true, "security", "Prerit", "Prerit");

            // Add the entity to the database
            var response = await _cosmosDBServices.AddSecurityUser(securityEntity);

            // Map the response back to a DTO
            return _mapper.Map<SecurityDTO>(response);
        }

        public async Task<SecurityDTO> GetSecurityById(string id)
        {
            var security = await _cosmosDBServices.GetSecurityById(id); // Call non-generic method
            return _mapper.Map<SecurityDTO>(security);
        }

        public async Task<SecurityDTO> UpdateSecurity(string id, SecurityDTO securityModel)
        {
            var securityEntity = await _cosmosDBServices.GetSecurityById(id);
            if (securityEntity == null)
            {
                throw new Exception("Security not found");
            }
            securityEntity = _mapper.Map<SecurityEntity>(securityModel);
            securityEntity.Id = id;
            var response = await _cosmosDBServices.UpdateSecurityUser(securityEntity);
            return _mapper.Map<SecurityDTO>(response);
        }

        public async Task<string> DeleteSecurity(string id)
        {
            /*await _cosmosDBServices.DeleteSecurity(id);*/
            var securityUserToDelete = await _cosmosDBServices.GetManagerById(id);
            securityUserToDelete.Active = false;
            securityUserToDelete.Archived = true;
            await _cosmosDBServices.UpdateManager(securityUserToDelete);

            securityUserToDelete.Initialize(false, Credentials.VisitorDocumnetType, "Admin", "Admin");
            securityUserToDelete.Active = false;
            securityUserToDelete.Archived = true;

            await _cosmosDBServices.AddManager(securityUserToDelete);
            return "Record Deleted Successfully...";
        }

        public async Task<SecurityDTO> LoginSecurityUser(string email, string password)
        {
            // Fetch the manager entity by email
            var securityUser = await _cosmosDBServices.GetSecurityUserByEmail(email);

            if (securityUser == null || securityUser.Password != password)
            {
                return null; // Credentials are invalid
            }

            // Map ManagerEntity to ManagerDTO
            var securityDto = new SecurityDTO
            {
                Id = securityUser.Id,
                Name = securityUser.Name,
                Email = securityUser.Email,
            };

            return securityDto;
        }
    }
}
