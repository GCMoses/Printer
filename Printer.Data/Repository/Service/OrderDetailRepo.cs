using Printer.Core.Models;
using Printer.Data;
using Printer.Data.Repository.Interface;
using Printer.Data.Repository.Service;

namespace Printer.Data.Repository.Service;

public class OrderDetailRepo : GenericRepo<OrderDetail>, IOrderDetailRepo
{
    private RepositoryContext _db;

    public OrderDetailRepo(RepositoryContext db) : base(db)
    {
        _db = db;
    }


    public void Update(OrderDetail obj)
    {
        _db.OrderDetail.Update(obj);
    }
}
