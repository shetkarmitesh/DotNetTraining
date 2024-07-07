using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Employee_Management_System.Controllers
{

    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class ImportExportController : Controller
    {
        private readonly IEmployeeBasicDetails _employeeBasicDetailsService;
        private readonly IEmployeeAdditionalDetails _employeeAdditionalDetailsService;
        public ImportExportController(IEmployeeBasicDetails basicDetailsService, IEmployeeAdditionalDetails additionalDetailsService)
        {
            _employeeBasicDetailsService = basicDetailsService;
            _employeeAdditionalDetailsService = additionalDetailsService;
        }

        private string GetStringFromCell(ExcelWorksheet worksheet, int row, int column)
        {
            var cellValue = worksheet.Cells[row, column].Value;
            return cellValue?.ToString()?.Trim();
        }


        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty or null");
            }

            var employees = new List<EmployeeBasicDetailsDTO>();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
                    if (worksheet == null)
                    {
                        return BadRequest("Excel file does not contain a worksheet!");
                    }
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var HouseNumberString = GetStringFromCell(worksheet, row, 10);
                        int HouseNumber;
                        if (!int.TryParse(HouseNumberString, out HouseNumber))
                        {
                            return BadRequest($"Invalid HouseNumber at row {row}");
                        }
                        var address = new Address
                        {
                            HouseNumber = HouseNumber,
                            SocietyName = GetStringFromCell(worksheet, row, 11),
                            City = GetStringFromCell(worksheet, row, 12),
                            State = GetStringFromCell(worksheet, row, 13),
                            Country = GetStringFromCell(worksheet, row, 14),
                            Pincode = GetStringFromCell(worksheet, row, 15)
                        };
                        var employeeBasicDetails = new EmployeeBasicDetailsDTO
                        {
                            EmployeeID = GetStringFromCell(worksheet, row, 2),
                            Salutory = GetStringFromCell(worksheet, row, 3),
                            FirstName = GetStringFromCell(worksheet, row, 4),
                            MiddleName = GetStringFromCell(worksheet, row, 5),
                            LastName = GetStringFromCell(worksheet, row, 6),
                            NickName = GetStringFromCell(worksheet, row, 7),
                            Email = GetStringFromCell(worksheet, row, 8),
                            Mobile = GetStringFromCell(worksheet, row, 9),
                            Address = address,
                            Role = GetStringFromCell(worksheet, row, 15),
                            ReportingManagerUId = GetStringFromCell(worksheet, row, 16),
                            ReportingManagerName = GetStringFromCell(worksheet, row, 17),
                        };
                        //adding employee basic details
                        var employeeAdded = await _employeeBasicDetailsService.AddEmployeeBasicDetails(employeeBasicDetails);
                        employees.Add(employeeAdded);// added to list to display results to added emplyee in db
                    }
                }

            }
            /*return Ok("Excel data imported successfully!\n"+employees);*/
            return Ok(employees);
        }
        [HttpGet]
        public async Task<IActionResult> Export()
        {
            //get all employees details
            var basicDetails = await _employeeBasicDetailsService.GetAllEmployeeBasicDetails();

            var additionalDetails = await _employeeAdditionalDetailsService.GetAllEmployeeAdditionalDetails();
            var employeesData = from basic in basicDetails join additional in additionalDetails on basic.EmployeeID equals additional.EmployeeBasicDetailsUId
                                select new
                                {

                                    EmployeeID = basic.EmployeeID,
                                    FirstName = basic.FirstName,
                                    LastName = basic.LastName,
                                    Email = basic.Email,
                                    Phone = basic.Mobile,
                                    ReportingManagerName = basic.ReportingManagerName,
                                    DateOfBirth = additional.PersonalDetails.DateOfBirth,
                                    DateOfJoining = additional.WorkInformation.DateOfJoining
                                };
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("EmployeesDetails");
                //headers
                var rowIndex = 1;
                worksheet.Cells[rowIndex, 1].Value = "Sr.No";
                worksheet.Cells[rowIndex, 2].Value = "EmployeeID";
                worksheet.Cells[rowIndex, 3].Value = "First Name";
                worksheet.Cells[rowIndex, 4].Value = "Last Name";
                worksheet.Cells[rowIndex, 5].Value = "Email";
                worksheet.Cells[rowIndex, 6].Value = "Phone No";
                worksheet.Cells[rowIndex, 7].Value = "Reporting Manager Name";
                worksheet.Cells[rowIndex, 8].Value = "Date Of Birth";
                worksheet.Cells[rowIndex, 9].Value = "Date of Joining";


                //header styling 
                using (var range = worksheet.Cells[1, 1, 1, 9])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType =ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);

                }
                    rowIndex = 2;
                foreach (var data in employeesData)
                {
                    worksheet.Cells[rowIndex, 1].Value = rowIndex-1;
                    worksheet.Cells[rowIndex, 2].Value = data.FirstName;
                    worksheet.Cells[rowIndex, 3].Value = data.LastName;
                    worksheet.Cells[rowIndex, 4].Value = data.Email;
                    worksheet.Cells[rowIndex, 5].Value = data.Phone;
                    worksheet.Cells[rowIndex, 6].Value = data.ReportingManagerName;
                    worksheet.Cells[rowIndex, 7].Value = data.DateOfBirth.ToShortDateString();
                    worksheet.Cells[rowIndex, 8].Value = data.DateOfJoining.ToShortDateString();
                    rowIndex++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "employeesDetails.xlsx");
                
            }
        }
        /*
                [HttpGet]
                 public async Task<IActionResult> ExportAllBasicAndAdditionalDetails()
                 {
                     var basicDetails = await _employeeBasicDetailsService.GetAllEmployeeBasicDetails();

                     var additionalDetails = await _employeeAdditionalDetailsService.GetAllEmployeeAdditionalDetails();


                     *//*var employeesData = from basic in basicDetails
                                         join additional in additionalDetails on basic.EmployeeID equals additional.EmployeeBasicDetailsUId
                                         select new
                                         {

                                             EmployeeID = basic.EmployeeID,
                                             Salutory = basic.Salutory,
                                             FirstName = basic.FirstName,
                                             MiddleName = basic.MiddleName,
                                             LastName = basic.LastName,
                                             Email = basic.Email,
                                             AlternateEmail = additional.AlternateEmail,
                                             Phone = basic.Mobile,
                                             AlternatePhone=additional.AlternateMobile,
                                             Role = basic.Role,  
                                             ReportingManagerUID= basic.ReportingManagerUId,
                                             ReportingManagerName = basic.ReportingManagerName,
                                             Address = $"{basic.Address.HouseNumber}, {basic.Address.SocietyName}, {basic.Address.Pincode}, {basic.Address.City}, {basic.Address.State}, {basic.Address.Country}",
                                            *//* DateOfBirth = additional.PersonalDetails?.DateOfBirth.ToString("yyyy-MM-dd"),
                                             DateOfJoining = additional.WorkInformation?.DateOfJoining.ToString("yyyy-MM-dd"),*//*
                                             WorkInformation = additional.WorkInformation != null ? $"{additional.WorkInformation.DesignationName}, {additional.WorkInformation.DepartmentName}, {additional.WorkInformation.LocationName}, {additional.WorkInformation.EmployeeStatus}, {additional.WorkInformation.SourceOfHire}" : "",
                                             // PersonalDetails as formatted string
                                             PersonalDetails = additional.PersonalDetails != null ? $"{additional.PersonalDetails?.DateOfBirth.ToString("yyyy-MM-dd")}, {additional.PersonalDetails.Age}, {additional.PersonalDetails.Religion}, {additional.PersonalDetails.Caste}, {additional.PersonalDetails.MaritalStatus}, {additional.PersonalDetails.BloodGroup}, {additional.PersonalDetails.Height}, {additional.PersonalDetails.Weight}" : "",
                                             // IdentityInformation as formatted string
                                             IdentityInformation = additional.IdentityInformation != null ? $"{additional.IdentityInformation.Aadhar}, {additional.IdentityInformation.Nationality}, {additional.IdentityInformation.PassportNumber}, {additional.IdentityInformation.PFNumber}" : "",
                                         };
         *//*
                     var employeesData = from basic in basicDetails
                                         join additional in additionalDetails on basic.EmployeeID equals additional.EmployeeBasicDetailsUId
                                         select new
                                         {
                                             basic,
                                             additional.WorkInformation,
                                             additional.PersonalDetails,
                                             additional.IdentityInformation,
                                         };

                     ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                     using (var package = new ExcelPackage())
                     {
                         var worksheet = package.Workbook.Worksheets.Add("EmployeesDetails");
                         //headers
                         var rowIndex = 1;
                         int colIndex = 1;
                         // Basic details headers
                         worksheet.Cells[rowIndex, colIndex++].Value = "Sr.No";
                         worksheet.Cells[rowIndex, colIndex++].Value = "EmployeeID";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Salutory";
                         worksheet.Cells[rowIndex, colIndex++].Value = "First Name";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Middle Name";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Last Name";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Email";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Alternate Email";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Phone No";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Alternate Phone No";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Role";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Reporting Manager UID";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Reporting Manager Name";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Address";

                         // Work information headers (assuming properties)
                         worksheet.Cells[rowIndex, colIndex++].Value = "Designation Name";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Department Name";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Location Name";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Employee Status";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Source Of Hire";

                         // Personal details headers (assuming properties)
                         worksheet.Cells[rowIndex, colIndex++].Value = "Date Of Birth";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Age";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Religion";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Caste";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Marital Status";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Blood Group";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Height";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Weight";

                         // Identity information headers (assuming properties)
                         worksheet.Cells[rowIndex, colIndex++].Value = "Aadhar";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Nationality";
                         worksheet.Cells[rowIndex, colIndex++].Value = "Passport Number";
                         worksheet.Cells[rowIndex, colIndex++].Value = "PF Number";

                         rowIndex++;
                         foreach (var data in employeesData)
                         {
                             worksheet.Cells[rowIndex, 1].Value = rowIndex - 1;
                             worksheet.Cells[rowIndex, 2].Value = data.FirstName;
                             worksheet.Cells[rowIndex, 2].Value = data.MiddleName;
                             worksheet.Cells[rowIndex, 3].Value = data.LastName;
                             worksheet.Cells[rowIndex, 4].Value = data.Email;
                             worksheet.Cells[rowIndex, 4].Value = data.AlternateEmail;
                             worksheet.Cells[rowIndex, 5].Value = data.Phone;
                             worksheet.Cells[rowIndex, 5].Value = data.AlternatePhone;
                             worksheet.Cells[rowIndex, 10].Value = data.Role;
                             worksheet.Cells[rowIndex, 11].Value = data.ReportingManagerUID;
                             worksheet.Cells[rowIndex, 12].Value = data.ReportingManagerName;
                             worksheet.Cells[rowIndex, 13].Value = data.Address;
                             *//*worksheet.Cells[rowIndex, 14].Value = data.DateOfBirth;
                             worksheet.Cells[rowIndex, 15].Value = data.DateOfJoining;*//*
                             worksheet.Cells[rowIndex, 14].Value = data.WorkInformation;
                             worksheet.Cells[rowIndex, 15].Value = data.PersonalDetails;
                             worksheet.Cells[rowIndex, 16].Value = data.IdentityInformation;
                             rowIndex++;
                         }

                         var stream = new MemoryStream();
                         package.SaveAs(stream);
                         stream.Position = 0;
                         return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "employeesDetails.xlsx");

                     }


                     }*/

        [HttpGet]
        public async Task<IActionResult> ExportAllBasicAndAdditionalDetails()
        {
            var basicDetails = await _employeeBasicDetailsService.GetAllEmployeeBasicDetails();
            var additionalDetails = await _employeeAdditionalDetailsService.GetAllEmployeeAdditionalDetails();

            var employeesData = from basic in basicDetails
                                join additional in additionalDetails on basic.EmployeeID equals additional.EmployeeBasicDetailsUId
                                select new
                                {
                                    basic,
                                    additional,
                                    additional.WorkInformation,
                                    additional.PersonalDetails,
                                    additional.IdentityInformation,
                                };

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("EmployeesDetails");

                // Headers
                var headers = new string[]
                {
            "Sr.No", "EmployeeID", "Salutory", "First Name", "Middle Name", "Last Name",
            "Email", "Alternate Email", "Phone No", "Alternate Phone No", "Role",
            "Reporting Manager UID", "Reporting Manager Name", "Address",
            "Designation Name", "Department Name", "Location Name", "Employee Status", "Source Of Hire",
            "Date Of Birth", "Age", "Religion", "Caste", "Marital Status", "Blood Group", "Height", "Weight",
            "Aadhar", "Nationality", "Passport Number", "PF Number"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                int rowIndex = 2;
                foreach (var data in employeesData)
                {
                    worksheet.Cells[rowIndex, 1].Value = rowIndex - 1;
                    worksheet.Cells[rowIndex, 2].Value = data.basic.EmployeeID;
                    worksheet.Cells[rowIndex, 3].Value = data.basic.Salutory;
                    worksheet.Cells[rowIndex, 4].Value = data.basic.FirstName;
                    worksheet.Cells[rowIndex, 5].Value = data.basic.MiddleName;
                    worksheet.Cells[rowIndex, 6].Value = data.basic.LastName;
                    worksheet.Cells[rowIndex, 7].Value = data.basic.Email;
                    worksheet.Cells[rowIndex, 8].Value = data.additional.AlternateEmail;
                    worksheet.Cells[rowIndex, 9].Value = data.basic.Mobile;
                    worksheet.Cells[rowIndex, 10].Value = data.additional.AlternateMobile;
                    worksheet.Cells[rowIndex, 11].Value = data.basic.Role;
                    worksheet.Cells[rowIndex, 12].Value = data.basic.ReportingManagerUId;
                    worksheet.Cells[rowIndex, 13].Value = data.basic.ReportingManagerName;
                    worksheet.Cells[rowIndex, 14].Value = $"{data.basic.Address.HouseNumber}, {data.basic.Address.SocietyName}, {data.basic.Address.Pincode}, {data.basic.Address.City}, {data.basic.Address.State}, {data.basic.Address.Country}";

                    // Work information
                    worksheet.Cells[rowIndex, 15].Value = data.WorkInformation?.DesignationName;
                    worksheet.Cells[rowIndex, 16].Value = data.WorkInformation?.DepartmentName;
                    worksheet.Cells[rowIndex, 17].Value = data.WorkInformation?.LocationName;
                    worksheet.Cells[rowIndex, 18].Value = data.WorkInformation?.EmployeeStatus;
                    worksheet.Cells[rowIndex, 19].Value = data.WorkInformation?.SourceOfHire;

                    // Personal details
                    worksheet.Cells[rowIndex, 20].Value = data.PersonalDetails?.DateOfBirth.ToString("yyyy-MM-dd");
                    worksheet.Cells[rowIndex, 21].Value = data.PersonalDetails?.Age;
                    worksheet.Cells[rowIndex, 22].Value = data.PersonalDetails?.Religion;
                    worksheet.Cells[rowIndex, 23].Value = data.PersonalDetails?.Caste;
                    worksheet.Cells[rowIndex, 24].Value = data.PersonalDetails?.MaritalStatus;
                    worksheet.Cells[rowIndex, 25].Value = data.PersonalDetails?.BloodGroup;
                    worksheet.Cells[rowIndex, 26].Value = data.PersonalDetails?.Height;
                    worksheet.Cells[rowIndex, 27].Value = data.PersonalDetails?.Weight;

                    // Identity information
                    worksheet.Cells[rowIndex, 28].Value = data.IdentityInformation?.Aadhar;
                    worksheet.Cells[rowIndex, 29].Value = data.IdentityInformation?.Nationality;
                    worksheet.Cells[rowIndex, 30].Value = data.IdentityInformation?.PassportNumber;
                    worksheet.Cells[rowIndex, 31].Value = data.IdentityInformation?.PFNumber;

                    rowIndex++;
                }

                // Stream the Excel package to the client as a file download
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "employeesDetails.xlsx");
            }
        }


    }
}
