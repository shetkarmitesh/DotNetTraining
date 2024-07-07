namespace Employee_Management_System.Common
{
    public class Credentials
    {
        public static readonly string databaseName = Environment.GetEnvironmentVariable("databaseName");
        public static readonly string containerName = Environment.GetEnvironmentVariable("containername");
        public static readonly string CosmosEndpoint = Environment.GetEnvironmentVariable("cosmosURL");
        public static readonly string PrimaryKey = Environment.GetEnvironmentVariable("primaryKey");

        internal static readonly string EmployeeUrl = Environment.GetEnvironmentVariable("employeeUrl");
        internal static readonly string AddEmployeeEndPoint = "/api/EmployeeBasicDetails/AddEmployeeBasicDetails";
        internal static readonly string GetAllEmployeesEndPoint = "api/EmployeeBasicDetails/GetAllEmployeeBasicDetails";
        
        internal static readonly string VisitorUrl = Environment.GetEnvironmentVariable("visitorUrl");
        internal static readonly string AddVisitorEndPoint = "/api/Visitor/AddVisitor";
        internal static readonly string GetAllVisitorEndPoint = "/api/Visitor/GetAllVisitors";



    }
}
