using Employee_Management_System.Common;
using Employee_Management_System.Entities;

namespace Employee_Management_System.DTOs
{
    public class EmployeeAdditionalDetailsDTO 
    {
        public string EmployeeBasicDetailsUId { get; set; }
        public string AlternateEmail { get; set; }
        public string AlternateMobile { get; set; }
        public WorkInfo_ WorkInformation { get; set; }
        public PersonalDetails_ PersonalDetails { get; set; }
        public IdentityInfo_ IdentityInformation { get; set; }
    }
   
}
