using VisitorSecurityClearanceSystem.Common;
using VisitorSecurityClearanceSystem.CosmosDB;
using VisitorSecurityClearanceSystem.DTOs;
using VisitorSecurityClearanceSystem.Interfaces;
using AutoMapper;
using VisitorSecurityClearanceSystem.Entities;
using System.Drawing;
using System.Xml.Linq;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
namespace VisitorSecurityClearanceSystem.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly ICosmosDBServices _cosmosDBServices;
        private readonly IMapper _mapper;

        public VisitorService(ICosmosDBServices cosmosDBServices, IMapper mapper)
        {
            _cosmosDBServices = cosmosDBServices;
            _mapper = mapper;
        }

        public async Task<VisitorDTO> AddVisitor(VisitorDTO visitorDTO)
        {
            var existingVisitor = await _cosmosDBServices.GetVisitorByEmail(visitorDTO.Email);
            if (existingVisitor != null)
            {
                throw new InvalidOperationException("A visitor already exists with this email.");
            }

            var visitor = _mapper.Map<VisitorEntity>(visitorDTO);
            visitor.Initialize(true, Credentials.VisitorDocumnetType, "Admin", "Admin");

            List<OfficeEntity> officeUser = await _cosmosDBServices.GetAllOfficeUser();
            foreach (var officer in officeUser)
            {
                if (officer.CompanyName == visitorDTO.CompanyName)
                {
                    //prepare email
                    string subject = "Visitor Registration Approval Request";
                    /*string toEmail = "tempvirajtapkir1800@gmail.com"; */
                    string toEmail = officer.Email;
                    string userName = "Manager (Office User)";
                    // Construct the email message with visitor's details
                    string message = $"Dear {userName},\n\n" +
                                     $"A new visitor has registered and is awaiting your approval.\n\n" +
                                     $"Visitor Details:\n" +
                                     $"Name: {visitorDTO.Name}\n" +
                                     $"Contact Number: {visitorDTO.Phone}\n" +
                                     $"Email: {visitorDTO.Email}\n" +
                                     $"Purpose of Visit: {visitorDTO.Purpose}\n\n" +
                                     "Please review the details and approve or reject the request.\n\n" +
                                     "Thank you,\nVisitor Management System";

                    // Sending the email
                    EmailSender emailSender = new EmailSender();
                    await emailSender.SendEmail(subject, toEmail, userName, message);

                }
            }
            


            var response = await _cosmosDBServices.AddVisitor(visitor);


            var responseDTO = _mapper.Map<VisitorDTO>(response);
            return responseDTO;
        }

        public async Task<List<VisitorDTO>> GetAllVisitors()
        {
            var visitors = await _cosmosDBServices.GetAllVisitors();
            var visitorDTOs = new List<VisitorDTO>();
            foreach (var visitor in visitors)
            {
                var visitorDTO = _mapper.Map<VisitorDTO>(visitor);
                visitorDTOs.Add(visitorDTO);
            }
            return visitorDTOs;
        }
        public async Task<VisitorDTO> GetVisitorByUId(string uId)
        {
            var visitor = await _cosmosDBServices.GetVisitorByUId(uId);
            
            var visitorDTO = _mapper.Map<VisitorDTO>(visitor);
            return visitorDTO;
        }
        public async Task<List<VisitorDTO>> GetVisitorsByStatus(bool status)
        {
            var visitors = await _cosmosDBServices.GetVisitorByStatus(status);
            var visitorDTOs = new List<VisitorDTO>();
            foreach (var visitor in visitors)
            {

                var visitorDTO = _mapper.Map<VisitorDTO>(visitor);
                visitorDTOs.Add(visitorDTO);
            }
            return visitorDTOs;
           
        }


        public async Task<VisitorDTO> UpdateVisitor(string uId, VisitorDTO visitorDTO)
        {
            var visitorEntity = await _cosmosDBServices.GetVisitorByUId(uId);
            if (visitorEntity == null)
            {
                throw new Exception("Visitor not found");
            }
            visitorEntity.Active = false;
            visitorEntity.Archived = true;
            await _cosmosDBServices.ReplaceAsync(visitorEntity);

            visitorEntity.Initialize(false, Credentials.VisitorDocumnetType, "Admin", "Admin");
            visitorEntity.UId=visitorDTO.UId;
            visitorEntity.Name=visitorDTO.Name;
            visitorEntity.Email=visitorDTO.Email;
            visitorEntity.Phone=visitorDTO.Phone;
            visitorEntity.PassStatus=visitorDTO.PassStatus;
            visitorEntity.Address=visitorDTO.Address;
            visitorEntity.Role=visitorDTO.Role;
            visitorEntity.CompanyName=visitorDTO.CompanyName;
            visitorEntity.Purpose=visitorDTO.Purpose;
            visitorEntity.EntryTime=visitorDTO.EntryTime;
            visitorEntity.ExitTime=visitorDTO.ExitTime;
            //error in mapping
            var response = await _cosmosDBServices.AddVisitor(visitorEntity);

            var responseDTO = _mapper.Map<VisitorDTO>(response);
            
            return responseDTO;
        }
        public async Task<VisitorDTO> UpdateVisitorStatus(string visitorUId, bool newStatus)
        {
            var visitor = await _cosmosDBServices.GetVisitorByUId(visitorUId);
            if (visitor == null)
            {
                throw new Exception("Visitor not found");
            }
            visitor.PassStatus = newStatus;
            await _cosmosDBServices.UpdateVisitor(visitor);

            //email mapping
            // Prepare email details
            string subject = "Your Visitor Status Has Been Updated";
            string toEmail = visitor.Email;  // Send to visitor's email
            string userName = visitor.Name;

            // Construct the email message with the new status details
            string message = $"Dear {userName},\n\n" +
                             $"We wanted to inform you that your visitor status has been updated.\n\n" +
                             $"New Status: {newStatus}\n\n" +
                             "If you have any questions or need further assistance, please contact us.\n\n" +
                             "Thank you,\nVisitor Management System";

            // If the status is true, generate the PDF and attach it to the email
            byte[] pdfBytes = null;
            if (newStatus)
            {
                pdfBytes = GenerateVisitorPassPdf(visitor);
            }

            // Send the email with or without the PDF attachment
            EmailSender emailSender = new EmailSender();
            await emailSender.SendEmail(subject, toEmail, userName, message, pdfBytes);

            var response = _mapper.Map<VisitorDTO>(visitor); ;
            
            return response;
        }
        public async Task<string> DeleteVisitor(string uId)
        {
           /* await _cosmosDBServices.DeleteVisitor(id);*/
            var visitorToDelete = await _cosmosDBServices.GetVisitorByUId(uId);
            visitorToDelete.Active = false;
            visitorToDelete.Archived = true;
            await _cosmosDBServices.UpdateVisitor(visitorToDelete);

            visitorToDelete.Initialize(false, Credentials.VisitorDocumnetType, "Admin", "Admin");
            visitorToDelete.Active = false;
            visitorToDelete.Archived = true;

            await _cosmosDBServices.AddVisitor(visitorToDelete);
            return "Record Deleted Successfully...";
        }

        private byte[] GenerateVisitorPassPdf(VisitorEntity visitor)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Create a new PDF document
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Define fonts
                XFont titleFont = new XFont("Arial", 20, XFontStyle.Bold);
                XFont normalFont = new XFont("Arial", 12);

                // Draw title
                gfx.DrawString("Visitor Pass", titleFont, XBrushes.Black, new XRect(0, 20, page.Width.Point, page.Height.Point), XStringFormats.Center);

                // Draw visitor details
                int yOffset = 60;
                gfx.DrawString($"Name: {visitor.Name}", normalFont, XBrushes.Black, new XRect(50, yOffset, page.Width.Point - 100, page.Height.Point), XStringFormats.TopLeft);
                yOffset += 20;
                gfx.DrawString($"Email: {visitor.Email}", normalFont, XBrushes.Black, new XRect(50, yOffset, page.Width.Point - 100, page.Height.Point), XStringFormats.TopLeft);
                yOffset += 20;
                gfx.DrawString($"Phone: {visitor.Phone}", normalFont, XBrushes.Black, new XRect(50, yOffset, page.Width.Point - 100, page.Height.Point), XStringFormats.TopLeft);
                yOffset += 20;
                gfx.DrawString($"Purpose of Visit: {visitor.Purpose}", normalFont, XBrushes.Black, new XRect(50, yOffset, page.Width.Point - 100, page.Height.Point), XStringFormats.TopLeft);

                // Save the PDF to memory stream
                document.Save(ms);
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        public async Task<List<VisitorDTO>> SearchVisitors(string name = null, string company = null, DateTime? fromDate = null, DateTime? toDate = null, bool? pass = null)
        {
            var query = await _cosmosDBServices.GetAllVisitors();
            var visitorDTOs = new List<VisitorDTO>();
            foreach (var visitor in query)
            {
                if (MatchesCriteria(visitor, name, company, fromDate, toDate,pass))
                {
                    var visitorDTO = _mapper.Map<VisitorDTO>(visitor);
                    visitorDTOs.Add(visitorDTO);
                }
            }

            return visitorDTOs;

        }
        private bool MatchesCriteria(VisitorEntity visitor, string name,string company, DateTime? fromDate, DateTime? toDate, bool? pass = null)
        {
            bool match = true;
            if (!string.IsNullOrEmpty(name) && !visitor.Name.ToLower().Contains(name.ToLower()))
            {
                match = false;
            }

            if (!string.IsNullOrEmpty(company) && !visitor.CompanyName.ToLower().Contains(company.ToLower()))
            {
                match = false;
            }

            if (fromDate.HasValue && visitor.EntryTime < fromDate)
            {
                match = false;
            }

            if (toDate.HasValue && visitor.ExitTime > toDate)
            {
                match = false;
            }

            // Check for Pass field (if provided)
            if (pass.HasValue)
            {
                match = match && visitor.PassStatus == pass.Value; // Use strict equality for bool
            }
            return match;
        }


        public async Task<VisitorDTO> AddVisitorByMakePostRequest(VisitorDTO visitor)
        {
            var serialObj = JsonConvert.SerializeObject(visitor);
            var requestObj = await HttpClientHelper.MakePostRequest(Credentials.EmployeeUrl, Credentials.AddEmployeeEndPoint, serialObj);
            var responseObj = JsonConvert.DeserializeObject<VisitorDTO>(requestObj);
            return responseObj;

        }

        public async Task<IEnumerable<VisitorDTO>> GetAllEmployeesBasicDetails()
        {
            var responseString = await HttpClientHelper.MakeGetRequest(Credentials.EmployeeUrl, Credentials.GetAllEmployeesEndPoint);
            var employees = JsonConvert.DeserializeObject<IEnumerable<VisitorDTO>>(responseString);
            return employees;
        }
    }
}
