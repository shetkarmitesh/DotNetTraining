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
        public async Task<SecurityDTO> AddSecurity(SecurityDTO securityDTO)
        {

            // Map the DTO to an Entity
            var securityEntity = _mapper.Map<SecurityEntity>(securityDTO);

            // Initialize the Entity
            securityEntity.Initialize(true, "security", "Prerit", "Prerit");

            // Add the entity to the database
            var response = await _cosmosDBServices.AddSecurityUser(securityEntity);

            // Map the response back to a DTO
            return _mapper.Map<SecurityDTO>(response);
        }

        public async Task<SecurityDTO> GetSecurityByUId(string uId)
        {
            var security = await _cosmosDBServices.GetSecurityByUId(uId); // Call non-generic method
            return _mapper.Map<SecurityDTO>(security);
        }

        public async Task<SecurityDTO> UpdateSecurity(string uId, SecurityDTO securityDTO)
        {
            var securityEntity = await _cosmosDBServices.GetSecurityByUId(uId);
            if (securityEntity == null)
            {
                throw new Exception("Security not found");
            }
            securityEntity = _mapper.Map<SecurityEntity>(securityDTO);
            securityEntity.UId = uId;
            var response = await _cosmosDBServices.UpdateSecurityUser(securityEntity);
            return _mapper.Map<SecurityDTO>(response);
        }

        public async Task<string> DeleteSecurity(string uId)
        {
            /*await _cosmosDBServices.DeleteSecurity(id);*/
            var securityUserToDelete = await _cosmosDBServices.GetManagerByUId(uId);
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
                UId = securityUser.UId,
                Name = securityUser.Name,
                Email = securityUser.Email,
            };

            return securityDto;
        }
    }
}
