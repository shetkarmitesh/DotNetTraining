namespace VisitorSecurityClearanceSystem.Common
{
    public class Credentials
    {
        public static readonly string databaseName = Environment.GetEnvironmentVariable("databaseName");
        public static readonly string containerName = Environment.GetEnvironmentVariable("containername");
        public static readonly string CosmosEndpoint = Environment.GetEnvironmentVariable("cosmosURL");
        public static readonly string PrimaryKey = Environment.GetEnvironmentVariable("primaryKey");
        public static string VisitorDocumnetType = "Visitor";
        public static string ApiKey = Environment.GetEnvironmentVariable("apiKey");

        internal static readonly string EmployeeUrl = Environment.GetEnvironmentVariable("employeeUrl");
        internal static readonly string AddEmployeeEndPoint = "/api/EmployeeBasicDetails/AddEmployeeBasicDetails";
        internal static readonly string GetAllEmployeesEndPoint = "api/Employee/GetAllEmployeeBasicDetails";

    }
}
