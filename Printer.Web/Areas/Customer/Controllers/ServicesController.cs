using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Printer.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ServicesController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }

    }
}