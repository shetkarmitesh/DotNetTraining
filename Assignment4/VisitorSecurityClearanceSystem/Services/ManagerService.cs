using AutoMapper;
using VisitorSecurityClearanceSystem.Common;
using VisitorSecurityClearanceSystem.CosmosDB;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Entities;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.Services
{
    public class ManagerService : IManagerService
    {
        private readonly ICosmosDBServices _cosmosDBServices;
        private readonly IMapper _mapper;

        public ManagerService(ICosmosDBServices cosmosDBServices, IMapper mapper)
        {
            _cosmosDBServices = cosmosDBServices;
            _mapper = mapper;
        }
        public async Task<ManagerDTO> AddManager(ManagerDTO managerDTO)
        {

            // Map the DTO to an Entity
            var managerEntity = _mapper.Map<ManagerEntity>(managerDTO);

            // Initialize the Entity
            managerEntity.Initialize(true, "manager", "Prerit", "Prerit");

            // Add the entity to the database
            var response = await _cosmosDBServices.AddManager(managerEntity);

            // Map the response back to a DTO
            return _mapper.Map<ManagerDTO>(response);
        }

        public async Task<ManagerDTO> GetManagerByUId(string uId)
        {
            var security = await _cosmosDBServices.GetManagerByUId(uId); // Call non-generic method
            return _mapper.Map<ManagerDTO>(security);
        }

        public async Task<ManagerDTO> UpdateManager(string uId, ManagerDTO managerDTO)
        {
            var managerEntity = await _cosmosDBServices.GetManagerByUId(uId);
            if (managerEntity == null)
            {
                throw new Exception("Manager not found");
            }
            managerEntity = _mapper.Map<ManagerEntity>(managerDTO);
            managerEntity.UId = uId;
            var response = await _cosmosDBServices.UpdateManager(managerEntity);
            return _mapper.Map<ManagerDTO>(response);
        }

        public async Task<string> DeleteManager(string uId)
        {
            /* await _cosmosDBServices.DeleteManager(uId);*/
            var mangerToDelete = await _cosmosDBServices.GetManagerByUId(uId);
            mangerToDelete.Active = false;
            mangerToDelete.Archived = true;
            await _cosmosDBServices.UpdateManager(mangerToDelete);

            mangerToDelete.Initialize(false, Credentials.VisitorDocumnetType, "Admin", "Admin");
            mangerToDelete.Active = false;
            mangerToDelete.Archived = true;

            await _cosmosDBServices.AddManager(mangerToDelete);
            return "Record Deleted Successfully...";
        }
    }
}
