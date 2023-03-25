using Printer.Core.Models;
using Printer.Core.VM;
using Printer.Data.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using Printer.Utilities;
using Stripe;

namespace Printer.Web.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IEmailSender _emailSender;
    private readonly IUnitOfWork _unitOfWork;
    [BindProperty]
    public ShoppingCartVM ShoppingCartVM { get; set; }
    public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }

    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        ShoppingCartVM = new ShoppingCartVM()
        {
            ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.AppUserId == claim.Value,
            includeProperties: "Product"),
            OrderHeader = new()
        };
        foreach (var cart in ShoppingCartVM.ListCart)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }
        return View(ShoppingCartVM);
    }

    public IActionResult Summary()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        ShoppingCartVM = new ShoppingCartVM()
        {
            ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.AppUserId == claim.Value,
            includeProperties: "Product"),
            OrderHeader = new()
        };
        ShoppingCartVM.OrderHeader.AppUser = _unitOfWork.AppUser.GetFirstOrDefault(
            u => u.Id == claim.Value);

        ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.AppUser.Name;
        ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.AppUser.PhoneNumber;
        ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.AppUser.StreetAddress;
        ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.AppUser.City;
        ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.AppUser.State;
        ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.AppUser.PostalCode;



foreach (var cart in ShoppingCartVM.ListCart)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }
        return View(ShoppingCartVM);
    }

    [HttpPost]
    [ActionName("Summary")]
    [ValidateAntiForgeryToken]
    public IActionResult SummaryPOST()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.AppUserId == claim.Value,
            includeProperties: "Product");


        ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
        ShoppingCartVM.OrderHeader.AppUserId = claim.Value;



        foreach (var cart in ShoppingCartVM.ListCart)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }
        AppUser applicationUser = _unitOfWork.AppUser.GetFirstOrDefault(u => u.Id == claim.Value);

        if (applicationUser.CompanyId.GetValueOrDefault() == 0)
        {
            ShoppingCartVM.OrderHeader.PaymentStatus = Statics.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = Statics.StatusPending;
        }
        else
        {
            ShoppingCartVM.OrderHeader.PaymentStatus = Statics.PaymentStatusDelayedPayment;
            ShoppingCartVM.OrderHeader.OrderStatus = Statics.StatusApproved;
        }

        _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
        _unitOfWork.Save();
        foreach (var cart in ShoppingCartVM.ListCart)
        {
            OrderDetail orderDetail = new()
            {
                ProductId = cart.ProductId,
                OrderId = ShoppingCartVM.OrderHeader.Id,
                Price = cart.Price,
                Count = cart.Count
            };
            _unitOfWork.OrderDetail.Add(orderDetail);
            _unitOfWork.Save();
        }


        if (applicationUser.CompanyId.GetValueOrDefault() == 0)
        {
            //stripe settings 
             var domain = "https://localhost:7225/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in ShoppingCartVM.ListCart)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),//20.00 -> 2000
                        Currency = "zar",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        },

                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        else
        {
            return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });
        }
    }

    public IActionResult OrderConfirmation(int id)
    {
        OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id, includeProperties: "AppUser");
        if (orderHeader.PaymentStatus != Statics.PaymentStatusDelayedPayment)
        {
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            //check the stripe status
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStripePaymentID(id, orderHeader.SessionId, session.PaymentIntentId);
                _unitOfWork.OrderHeader.UpdateStatus(id, Statics.StatusApproved, Statics.PaymentStatusApproved);
                _unitOfWork.Save();
            }
        }

        //string htmlMessage = "<p>Digipix New Order Created</p>" +
        //                    "<p>Order ID: " + orderHeader.Id + "</p>" +
        //                    "<p>Order Date: " + orderHeader.OrderDate + "</p>" +
        //                    "<p>Order Total: " + orderHeader.OrderTotal + "</p>" +
        //                    "<p>Order Status: " + orderHeader.OrderStatus + "</p>" +
        //                    "<p>Payment Status: " + orderHeader.PaymentStatus + "</p>" +
        //                    "<p>Stripe Payment ID: " + orderHeader.PaymentIntentId + "</p>";
        string htmlMessage = System.IO.File.ReadAllText("C:/Users/PC/source/repos/Printer/Printer.Web/wwwroot/templates/OrderEmail.html")
  .Replace("[OrderID]", orderHeader.Id.ToString())
  .Replace("[OrderDate]", orderHeader.OrderDate.ToString("yyyy-MM-dd"))
  .Replace("[OrderTotal]", orderHeader.OrderTotal.ToString("C"))
  .Replace("[OrderStatus]", orderHeader.OrderStatus)
  .Replace("[PaymentStatus]", orderHeader.PaymentStatus)
  .Replace("[PaymentIntentID]", orderHeader.PaymentIntentId);


        _emailSender.SendEmailAsync(orderHeader.AppUser.Email, "New Order - DigiPix", htmlMessage);
        List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.AppUserId ==
        orderHeader.AppUserId).ToList();
        HttpContext.Session.Clear();
        _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
        _unitOfWork.Save();
        return View(id);
    }



    public IActionResult Plus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
        _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
        if (cart.Count <= 1)
        {
            _unitOfWork.ShoppingCart.Remove(cart);
        }
        else
        {
            _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
        }
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
        _unitOfWork.ShoppingCart.Remove(cart);
        _unitOfWork.Save();
        var count = _unitOfWork.ShoppingCart.GetAll(u => u.AppUserId == cart.AppUserId).ToList().Count;
        HttpContext.Session.SetInt32(Statics.SessionCart, count);
        return RedirectToAction(nameof(Index));
    }



    private double GetPriceBasedOnQuantity(double quantity, double price)
    {
        return price;
    }
}