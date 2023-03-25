using Printer.Core.Models;
using Printer.Data;
using Printer.Data.Repository.Interface;

namespace Printer.Data.Repository.Service;

public class ShoppingCartRepo : GenericRepo<ShoppingCart>, IShoppingCartRepo
{
    private RepositoryContext _db;

    public ShoppingCartRepo(RepositoryContext db) : base(db)
    {
        _db = db;
    }

    public int DecrementCount(ShoppingCart shoppingCart, int count)
    {
        shoppingCart.Count -= count;
        return shoppingCart.Count;
    }
    public int IncrementCount(ShoppingCart shoppingCart, int count)
    {
        shoppingCart.Count += count;
        return shoppingCart.Count;
    }

}