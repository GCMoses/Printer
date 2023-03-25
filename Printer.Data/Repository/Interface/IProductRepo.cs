using Printer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Data.Repository.Interface;

public interface IProductRepo : IGenericRepo<Product>
{
    void Update(Product obj);
}