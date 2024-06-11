using Azure;
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



        public async Task<ManagerEntity> AddManager(ManagerEntity managerEntity)
        {
            var response = await _container.CreateItemAsync<ManagerEntity>(managerEntity);
            return response;
        }
        public async Task<ManagerEntity> UpdateManager(ManagerEntity managerEntity)
        {
            var response = await _container.UpsertItemAsync(managerEntity);
            return response;
        }
        public async Task<ManagerEntity> GetManagerById(string id)
        {
                var response = _container.GetItemLinqQueryable<ManagerEntity>(true).Where(q => q.Id == id && q.Active && !q.Archived).FirstOrDefault();

                return response;

        }


        public async Task<SecurityEntity> AddSecurityUser(SecurityEntity securityEntity)
        {
            var response = await _container.CreateItemAsync<SecurityEntity>(securityEntity);
            return response;
        }
        public async Task<SecurityEntity> UpdateSecurityUser(SecurityEntity securityEntity)
        {
            var response = await _container.UpsertItemAsync(securityEntity);
            return response;
        }
        public async Task<SecurityEntity> GetSecurityUserByEmail(string email)
        {
            var response = _container.GetItemLinqQueryable<SecurityEntity>(true).Where(q => q.Email == email && q.Active && !q.Archived).FirstOrDefault();

            return response;
        }
        public async Task<SecurityEntity> GetSecurityById(string id)
        {

            var response = _container.GetItemLinqQueryable<SecurityEntity>(true).Where(q => q.Id == id && q.Active && !q.Archived).FirstOrDefault();
                return response;
           
        }

        public async Task<OfficeEntity> AddOfficeUser(OfficeEntity officeUserEntity)
        {
            var response = await _container.CreateItemAsync<OfficeEntity>(officeUserEntity);
            return response;
        }
        public async Task<OfficeEntity> UpdateOfficeUser(OfficeEntity officeUserEntity)
        {
            var response = await _container.UpsertItemAsync(officeUserEntity);
            return response;
        }
        public async Task<OfficeEntity> GetOfficeUserByEmail(string email)
        {
            var response = _container.GetItemLinqQueryable<OfficeEntity>(true).Where(q => q.Email == email && q.Active && !q.Archived).FirstOrDefault();

            return response;
        }
        public async Task<OfficeEntity> GetOfficeById(string id)
        {
                
            var query = _container.GetItemLinqQueryable<OfficeEntity>(true).Where(q => q.Id == id && q.Active && !q.Archived).FirstOrDefault();

                return query;
           
           
        }

    }
}
