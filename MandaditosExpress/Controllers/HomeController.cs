using Microsoft.AspNet.Identity.Owin;
using System;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MandaditosExpress.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.SendEmail = false;
            return View();
        }

        public ActionResult Index1()
        {
            ViewBag.SendEmail = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact( string email, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SmtpClient client = new SmtpClient();
                    client.EnableSsl = true;
                    SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.To.Add(smtpSection.From!= null ? smtpSection.From : "elmandaditoexpressni@gmail.com");
                    mailMessage.Subject = subject;
                    mailMessage.Body = email + ": " + message;

                    await client.SendMailAsync(mailMessage);

                    ViewBag.SendEmail = true;
                    return View("Index1");
                }

                ViewBag.SendEmail = false;
                return View("Index1");
            }
            catch(Exception ex)
            {
                ViewBag.SendEmail = false;
                throw ex;
            }
        }
    }
}