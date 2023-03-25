using Printer.Core.Models;
using Printer.Data.Repository.Interface;

namespace Printer.Data.Repository.Interface;
public interface IOrderDetailRepo : IGenericRepo<OrderDetail>
{
    void Update(OrderDetail obj);
}