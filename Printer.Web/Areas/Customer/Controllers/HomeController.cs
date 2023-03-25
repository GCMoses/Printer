using Printer.Core.Models;
using Printer.Data.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Printer.Utilities;
using X.PagedList;

namespace Printer.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _loggers;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _loggers = logger;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index(int? page)
        {
            const int pageSize = 3;
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category,DigiPixCover")
                                                .OrderByDescending(p => p.Id);
            var pageNumber = page ?? 1;
            var onePageOfProducts = products.ToPagedList(pageNumber, pageSize);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(onePageOfProducts);
        }


        [HttpGet]
        public IActionResult Details(int productId)
        {
            ShoppingCart cartObj = new()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,DigiPixCover"),
            };

            // Get IP address and port number
            var ipAddress = Request.Host.Host;
            var portNumber = Request.Host.Port.HasValue ? ":" + Request.Host.Port.Value : "";

            // Generate shareable link
            string shareableLink = $"{Request.Scheme}://{ipAddress}{portNumber}{Url.Action("Details", "Home", new { productId = productId })}";

            ViewData["ShareableLink"] = shareableLink;

            return View(cartObj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.AppUserId = claim.Value;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                u => u.AppUserId == claim.Value && u.ProductId == shoppingCart.ProductId);

            if (cartFromDb == null)
            {

                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(Statics.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.AppUserId == claim.Value).ToList().Count);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, shoppingCart.Count);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}