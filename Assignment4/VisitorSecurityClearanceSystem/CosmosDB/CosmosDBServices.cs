using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using VisitorSecurityClearanceSystem.Common;
using VisitorSecurityClearanceSystem.Entities;
using VisitorSecurityClearanceSystem.Interfaces;

namespace VisitorSecurityClearanceSystem.CosmosDB
{
    public class CosmosDBServices : ICosmosDBServices
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public CosmosDBServices()
        {
            _cosmosClient = new CosmosClient(Credentials.CosmosEndpoint, Credentials.PrimaryKey);
            _container = _cosmosClient.GetContainer(Credentials.databaseName, Credentials.containerName);
        }
        public async Task<VisitorEntity> AddVisitor(VisitorEntity entity)
        {
            var response = await _container.CreateItemAsync<VisitorEntity>(entity);
            return response;
        }
        public async Task<VisitorEntity> UpdateVisitor(VisitorEntity  visitorEntity)
        {
            var response = await _container.UpsertItemAsync(visitorEntity);
            return response;    
        }
        public async Task<VisitorEntity> GetVisitorByEmail(string email)
        {
            var visitor = _container.GetItemLinqQueryable<VisitorEntity>(true).Where(q => q.Email == email && q.Active && !q.Archived).FirstOrDefault();

            return visitor;
        }
        public async Task<VisitorEntity> GetVisitorById(string id)
        {
            
                var visitor = _container.GetItemLinqQueryable<VisitorEntity>(true).Where(q => q.Id == id && q.Active && !q.Archived).FirstOrDefault();

                return visitor;
            
        }
        
        public async Task<List<VisitorEntity>> GetVisitorByStatus(bool status)
        {
            var visitors = _container.GetItemLinqQueryable<VisitorEntity>(true).Where(q => q.PassStatus == status && q.Active && !q.Archived).ToList();

            return visitors ;
        }
        public async Task<List<VisitorEntity>> GetAllVisitors()
        {
            var visitors = _container.GetItemLinqQueryable<VisitorEntity>(true).Where(s => s.DocumentType == "visitor" && s.Active && !s.Archived).ToList();

            return visitors ;
        }


    }
}
