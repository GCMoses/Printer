using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Printer.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class AboutController : Controller
{

    public IActionResult Index()
    {
        return View();
    }
}