
using Printer.Core.Models;
using Printer.Data.Repository.Interface;

namespace Printer.Data.Repository.Interface;
public interface IOrderHeaderRepo : IGenericRepo<OrderHeader>
{
    void Update(OrderHeader obj);
    void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
    void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
}
