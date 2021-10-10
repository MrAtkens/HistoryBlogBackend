using GeekBlog.Contracts.Options;
using System;
using System.Net.Mail;

namespace GeekBlog.DataAccess.Models.System
{
    public class MailSender
    {
        public void sendMail(Blog blog)
        {
            MailMessage mailMessage = new MailMessage(MailOption.Mail, MailOption.Mail);
            mailMessage.Subject = "Message from " + sendMailDto.FirstName + " mail address: " + sendMailDto.Email;
            mailMessage.Body = sendMailDto.Message;
            mailMessage.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential netCredential = new NetworkCredential(MailOption.Mail, MailOption.Password);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = netCredential;
            smtp.Port = 587;
            try
            {
                await smtp.SendMailAsync(mailMessage);
                return NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(500);
            }
        }
    }
}
