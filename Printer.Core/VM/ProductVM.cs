using Printer.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Printer.Core.VM;
public class ProductVM
{
    public Product Product { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> DigiPixCoverList { get; set; }
}