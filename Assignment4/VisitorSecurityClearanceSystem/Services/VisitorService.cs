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
            var response = await _cosmosDBServices.AddVisitor(visitor);

            //prepare email
            string subject = "Visitor Registration Approval Request";
            string toEmail = "tempvirajtapkir1800@gmail.com";  // Change to manager's email
            string userName = "Manager";
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
        public async Task<VisitorDTO> GetVisitorById(string UId)
        {
            var visitor = await _cosmosDBServices.GetVisitorById(UId);
            
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


        public async Task<VisitorDTO> UpdateVisitor(string id, VisitorDTO visitorModel)
        {
            var visitorEntity = await _cosmosDBServices.GetVisitorById(id);
            if (visitorEntity == null)
            {
                throw new Exception("Manager not found");
            }
            visitorEntity = _mapper.Map<VisitorEntity>(visitorModel); ;
            visitorEntity.Id = id;
            var response = await _cosmosDBServices.UpdateVisitor(visitorEntity);

            return _mapper.Map<VisitorDTO>(response);
        }
        public async Task<VisitorDTO> UpdateVisitorStatus(string visitorId, bool newStatus)
        {
            var visitor = await _cosmosDBServices.GetVisitorById(visitorId);
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
            /*return new VisitorDTO
            {
                Id = visitor.Id,
                Name = visitor.Name,
                Email = visitor.Email,
                PassStatus = visitor.PassStatus,
                // Map other properties as needed
            };*/
            return response;
        }
        public async Task<string> DeleteVisitor(string id)
        {
           /* await _cosmosDBServices.DeleteVisitor(id);*/
            var visitorToDelete = await _cosmosDBServices.GetVisitorById(id);
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

    }
}
