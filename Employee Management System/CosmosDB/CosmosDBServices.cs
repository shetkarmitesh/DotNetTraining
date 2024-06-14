using Employee_Management_System.Common;
using Employee_Management_System.Entities;
using Microsoft.Azure.Cosmos;

namespace Employee_Management_System.CosmosDB
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
        public async Task<EmployeeAdditionalDetails> AddEmployeeAdditionalDetails(EmployeeAdditionalDetails entity)
        {
            var response = await _container.CreateItemAsync(entity);
            return response;
        }
        public async Task<EmployeeAdditionalDetails> UpdateEmployeeAdditionalDetails(EmployeeAdditionalDetails Entity)
        {
            var response = await _container.UpsertItemAsync(Entity);
            return response;
        }

        public async Task<List<EmployeeAdditionalDetails>> GetAllEmployeeAdditionalDetails()
        {
            var visitors = _container.GetItemLinqQueryable<EmployeeAdditionalDetails>(true).Where(s => s.DocumentType == "employeeAdditionalDetails" && s.Active && !s.Archived).ToList();

            return visitors;
        }
        public async Task<EmployeeAdditionalDetails> GetAllEmployeeAdditionalDetailsById(string uId)
        {

            var employee = _container.GetItemLinqQueryable<EmployeeAdditionalDetails>(true).Where(q => q.EmployeeBasicDetailsUId == uId && q.Active && !q.Archived).FirstOrDefault();

            return employee;

        }
         public async Task<EmployeeBasicDetails> AddEmployeeBasicDetails(EmployeeBasicDetails entity)
        {
            var response = await _container.CreateItemAsync(entity);
            return response;
        }
        public async Task<EmployeeBasicDetails> UpdateEmployeeBasicDetails(EmployeeBasicDetails Entity)
        {
            var response = await _container.UpsertItemAsync(Entity);
            return response;
        }
        public async Task<List<EmployeeBasicDetails>> GetAllEmployeeBasicDetails()
        {
            var response = _container.GetItemLinqQueryable<EmployeeBasicDetails>(true).Where(s => s.DocumentType == "employeeBasicDetails" && s.Active && !s.Archived).ToList();

            return response;
        }
        public async Task<EmployeeBasicDetails> GetEmployeeBasicDetailsById(string id)
        {

            var response = _container.GetItemLinqQueryable<EmployeeBasicDetails>(true).Where(q => q.EmployeeID == id && q.Active && !q.Archived).FirstOrDefault();


            return response;

        }public async Task<EmployeeBasicDetails> GetEmployeeBasicDetailsByUId(string uId)
        {

            var response = _container.GetItemLinqQueryable<EmployeeBasicDetails>(true).Where(q => q.EmployeeBasicDetailsUId == uId && q.Active && !q.Archived).FirstOrDefault();


            return response;

        }

        

    }
}
