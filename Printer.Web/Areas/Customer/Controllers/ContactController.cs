using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Printer.Core.VM;

namespace Printer.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ContactController : Controller
    {
        private readonly IEmailSender _emailSender;

        public ContactController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new ContactVM();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactVM contactVM)
        {
            if (ModelState.IsValid)
            {
                string message = $"Name: {contactVM.Name}<br />Email: {contactVM.Email}<br />Phone: {contactVM.Phone}<br />Subject: {contactVM.Subject}<br />Message:<br />{contactVM.Message}";

                await _emailSender.SendEmailAsync("devsoluz@gmail.com", "New Contact Message", message);

                TempData["SuccessMessage"] = "Your message has been sent successfully.";

                return RedirectToAction(nameof(Index));
            }
            return View(contactVM);
        }
    }
}
