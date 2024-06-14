using Employee_Management_System.DTOs;
using Employee_Management_System.Entities;
using Employee_Management_System.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

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

                rowIndex=2;
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
    }
}
