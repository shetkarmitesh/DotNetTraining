﻿namespace Employee_Management_System.Entities
{
    public class WorkInfo_
    {
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string LocationName { get; set; }
        public string EmployeeStatus { get; set; } // Terminated, Active, Resigned etc
        public string SourceOfHire { get; set; }
        public DateTime DateOfJoining { get; set; }
    }
}
