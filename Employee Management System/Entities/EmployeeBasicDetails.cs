using System.Net;
using Employee_Management_System.Common;
using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;

namespace Employee_Management_System.Entities
{
    public class EmployeeBasicDetails: BaseEntities
    {
        public string Salutory { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string EmployeeID { get; set; }
        public string Role { get; set; }
        public string ReportingManagerUId { get; set; }
        public string ReportingManagerName { get; set; }
        public Address Address { get; set; }
    }

    public class EmployeeFilterCriteria
    {
        public EmployeeFilterCriteria()
        {
            Filters = new List<FilterCriteria>();
            Employees = new List<EmployeeBasicDetailsDTO>();
        }
        public int page { get; set; } //page number
        public int pageSize { get; set; } //records in one page
        public int TotalCount { get; set; } //total records in the db

        public List<FilterCriteria> Filters { get; set; } //pass filter
        public List<EmployeeBasicDetailsDTO> Employees { get; set; }
    }


    public class FilterCriteria
    {
        public string fieldName { get; set; }
        public List<string> fieldValue { get; set; }
    }
}
