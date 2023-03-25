using Printer.Core.Models;

namespace Printer.Data.Repository.Interface;
public interface IShoppingCartRepo : IGenericRepo<ShoppingCart>
{
    int IncrementCount(ShoppingCart shoppingCart, int count);
    int DecrementCount(ShoppingCart shoppingCart, int count);
}
