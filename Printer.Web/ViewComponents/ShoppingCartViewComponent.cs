
using Microsoft.AspNetCore.Mvc;
using Printer.Data.Repository.Interface;
using Printer.Utilities;
using System.Security.Claims;

namespace Printer.Web.ViewComponents;

public class ShoppingCartViewComponent : ViewComponent
{
    private readonly IUnitOfWork _unitOfWork;
    public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IViewComponentResult Invoke()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        if (claim != null)
        {
            if (HttpContext.Session.GetInt32(Statics.SessionCart) != null)
            {
                return View(HttpContext.Session.GetInt32(Statics.SessionCart));
            }
            else
            {
                HttpContext.Session.SetInt32(Statics.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.AppUserId == claim.Value).ToList().Count);
                return View(HttpContext.Session.GetInt32(Statics.SessionCart));
            }
        }
        else
        {
            HttpContext.Session.Clear();
            return View(0);
        }
    }

}
