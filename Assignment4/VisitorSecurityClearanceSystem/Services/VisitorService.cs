using VisitorSecurityClearanceSystem.Common;
using VisitorSecurityClearanceSystem.CosmosDB;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Interfaces;
using AutoMapper;
using VisitorSecurityClearanceSystem.Entities;
namespace VisitorSecurityClearanceSystem.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly ICosmosDBServices _cosmosDBServices;
        private readonly IMapper _mapper;

        public VisitorService(ICosmosDBServices cosmosDBServices, IMapper mapper)
        {
            _cosmosDBServices = cosmosDBServices;
            _mapper = mapper;
        }

        public async Task<VisitorDTO> AddVisitor(VisitorDTO visitorDTO)
        {
            var existingVisitor = await _cosmosDBServices.GetVisitorByEmail(visitorDTO.Email);
            if (existingVisitor != null)
            {
                throw new InvalidOperationException("A visitor already exists with this email.");
            }

            var visitor = _mapper.Map<VisitorEntity>(visitorDTO);
            visitor.Initialize(true, Credentials.VisitorDocumnetType, "Admin", "Admin");
            var response = await _cosmosDBServices.AddVisitor(visitor);

            //prepare email
            var responseDTO = _mapper.Map<VisitorDTO>(response);
            return responseDTO;
        }

        public async Task<List<VisitorDTO>> GetAllVisitors()
        {
            var visitors = await _cosmosDBServices.GetAllVisitors();
            var visitorDTOs = new List<VisitorDTO>();
            foreach (var visitor in visitors)
            {
               
                var visitorDTO = _mapper.Map<VisitorDTO>(visitor);
                visitorDTOs.Add(visitorDTO);
            }
            return visitorDTOs;
        }
        public async Task<VisitorDTO> GetVisitorById(string UId)
        {
            var visitor = await _cosmosDBServices.GetVisitorById(UId);
            
            var visitorDTO = _mapper.Map<VisitorDTO>(visitor);
            return visitorDTO;
        }
        public async Task<List<VisitorDTO>> GetVisitorsByStatus(bool status)
        {
            var visitors = await _cosmosDBServices.GetVisitorByStatus(status);
            var visitorDTOs = new List<VisitorDTO>();
            foreach (var visitor in visitors)
            {

                var visitorDTO = _mapper.Map<VisitorDTO>(visitor);
                visitorDTOs.Add(visitorDTO);
            }
            return visitorDTOs;
           
        }


        public async Task<VisitorDTO> UpdateVisitor(string id, VisitorDTO visitorModel)
        {
            var visitorEntity = await _cosmosDBServices.GetVisitorById(id);
            if (visitorEntity == null)
            {
                throw new Exception("Manager not found");
            }
            visitorEntity = _mapper.Map<VisitorEntity>(visitorModel); ;
            visitorEntity.Id = id;
            var response = await _cosmosDBServices.UpdateVisitor(visitorEntity);
            return _mapper.Map<VisitorDTO>(response);
        }
        public async Task<VisitorDTO> UpdateVisitorStatus(string visitorId, bool newStatus)
        {
            var visitor = await _cosmosDBServices.GetVisitorById(visitorId);
            if (visitor == null)
            {
                throw new Exception("Visitor not found");
            }
            visitor.PassStatus = newStatus;
            await _cosmosDBServices.UpdateVisitor(visitor);

            //email mapping

            var response = _mapper.Map<VisitorDTO>(visitor); ;
            /*return new VisitorDTO
            {
                Id = visitor.Id,
                Name = visitor.Name,
                Email = visitor.Email,
                PassStatus = visitor.PassStatus,
                // Map other properties as needed
            };*/
            return response;
        }
        public async Task<string> DeleteVisitor(string id)
        {
           /* await _cosmosDBServices.DeleteVisitor(id);*/
            var visitorToDelete = await _cosmosDBServices.GetVisitorById(id);
            visitorToDelete.Active = false;
            visitorToDelete.Archived = true;
            await _cosmosDBServices.UpdateVisitor(visitorToDelete);

            visitorToDelete.Initialize(false, Credentials.VisitorDocumnetType, "Admin", "Admin");
            visitorToDelete.Active = false;
            visitorToDelete.Archived = true;

            await _cosmosDBServices.AddVisitor(visitorToDelete);
            return "Record Deleted Successfully...";
        }

    }
}
