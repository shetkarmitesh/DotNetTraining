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
        public async Task<ManagerDTO> AddManager(ManagerDTO managerModel)
        {

            // Map the DTO to an Entity
            var managerEntity = _mapper.Map<ManagerEntity>(managerModel);

            // Initialize the Entity
            managerEntity.Initialize(true, "manager", "Prerit", "Prerit");

            // Add the entity to the database
            var response = await _cosmosDBServices.AddManager(managerEntity);

            // Map the response back to a DTO
            return _mapper.Map<ManagerDTO>(response);
        }

        public async Task<ManagerDTO> GetManagerById(string id)
        {
            var security = await _cosmosDBServices.GetManagerById(id); // Call non-generic method
            return _mapper.Map<ManagerDTO>(security);
        }

        public async Task<ManagerDTO> UpdateManager(string id, ManagerDTO managerModel)
        {
            var managerEntity = await _cosmosDBServices.GetManagerById(id);
            if (managerEntity == null)
            {
                throw new Exception("Manager not found");
            }
            managerEntity = _mapper.Map<ManagerEntity>(managerModel);
            managerEntity.Id = id;
            var response = await _cosmosDBServices.UpdateManager(managerEntity);
            return _mapper.Map<ManagerDTO>(response);
        }

        public async Task<string> DeleteManager(string id)
        {
            /* await _cosmosDBServices.DeleteManager(id);*/
            var mangerToDelete = await _cosmosDBServices.GetManagerById(id);
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
