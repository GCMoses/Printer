using Printer.Core.Models;
using Printer.Data;
using Printer.Data.Repository.Interface;
using Printer.Data.Repository.Service;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Printer.Data.Repository.Service;

public class UnitOfWork : IUnitOfWork
{
    private RepositoryContext _db;

    public UnitOfWork(RepositoryContext db)
    {
        _db = db;
        Category = new CategoryRepo(_db);
        DigiPixCover = new DigiPixCoverRepo(_db);
        Product = new ProductRepo(_db);
        Company = new CompanyRepo(_db);
        AppUser = new AppUserRepo(_db);
        ShoppingCart = new ShoppingCartRepo(_db);
        OrderHeader = new OrderHeaderRepo(_db);
        OrderDetail = new OrderDetailRepo(_db);
    }
    public ICategoryRepo Category { get; private set; }
    public IDigiPixCoverRepo DigiPixCover { get; private set; }
    public IProductRepo Product { get; private set; }
    public ICompanyRepo Company { get; private set; }
    public IShoppingCartRepo ShoppingCart { get; private set; }

    public IAppUserRepo AppUser { get; private set; }
    public IOrderHeaderRepo OrderHeader { get; private set; }
    public IOrderDetailRepo OrderDetail { get; private set; }


    public void Save()      
    {
        _db.SaveChanges();
    }
}