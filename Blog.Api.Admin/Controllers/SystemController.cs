using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BazarJok.Contracts.Dtos;
using BazarJok.Contracts.Options;
using BazarJok.Contracts.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BazarJok.Api.Admin.Controllers
{
    [Route("api/system/")]
    [EnableCors(CorsOrigins.FrontPolicy)]
    [ApiController]
    public class SystemController : ControllerBase
    {

        public SystemController()
        {
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(SendMailDto sendMailDto)
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