﻿using SendGrid.Helpers.Mail;
using System.Net.Mail;
using SendGrid;

namespace VisitorSecurityClearanceSystem.Services
{
    public class EmailSender
    {
        public async Task SendEmail(string subject,string toEmail, string userName,string message, byte[] pdfBytes=null)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("tempvirajtapkir1800@gmail.com", "VisitorSecurityClearanceSystem");
            var to = new EmailAddress(toEmail, userName);
            var plainTextContent = message;
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            // Attach the PDF if provided
            if (pdfBytes != null)
            {
                msg.AddAttachment("VisitorPass.pdf", Convert.ToBase64String(pdfBytes));
            }
            var response = await client.SendEmailAsync(msg);
        }   
    }
}
