using Printer.Core.Models;
using Printer.Data;
using Printer.Data.Repository.Interface;
using Printer.Data.Repository.Service;

namespace Printer.Data.Repository.Service;
public class OrderHeaderRepo : GenericRepo<OrderHeader>, IOrderHeaderRepo
{
    private RepositoryContext _db;

    public OrderHeaderRepo(RepositoryContext db) : base(db)
    {
        _db = db;
    }


    public void Update(OrderHeader obj)
    {
        _db.OrderHeaders.Update(obj);
    }

    public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
    {
        var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
        if (orderFromDb != null)
        {
            orderFromDb.OrderStatus = orderStatus;
            if (paymentStatus != null)
            {
                orderFromDb.PaymentStatus = paymentStatus;
            }
        }
    }

    public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
    {
        var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);

        orderFromDb.PaymentDate = DateTime.Now;
        orderFromDb.SessionId = sessionId;
        orderFromDb.PaymentIntentId = paymentIntentId;
    }
}