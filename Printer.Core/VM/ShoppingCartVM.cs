using Printer.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printer.Core.VM;
public class ShoppingCartVM
{
    public IEnumerable<ShoppingCart> ListCart { get; set; }
    public OrderHeader OrderHeader { get; set; }

}