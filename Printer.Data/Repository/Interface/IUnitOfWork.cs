using Printer.Data.Repository.Interface;

namespace Printer.Data.Repository.Interface;

public interface IUnitOfWork
{
    ICategoryRepo Category { get; }
    IDigiPixCoverRepo DigiPixCover { get; }
    IProductRepo Product { get; }
    ICompanyRepo Company { get; }
    IAppUserRepo AppUser { get; }
    IShoppingCartRepo ShoppingCart { get; }
    IOrderHeaderRepo OrderHeader { get; }
    IOrderDetailRepo OrderDetail { get; }

    void Save();
}
