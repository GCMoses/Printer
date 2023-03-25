using Printer.Core.Models;
using Printer.Data;
using Printer.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Printer.Data.Repository.Service;

public class ProductRepo : GenericRepo<Product>, IProductRepo
{
    private RepositoryContext _db;

    public ProductRepo(RepositoryContext db) : base(db)
    {
        _db = db;
    }


    public void Update(Product obj)
    {
        var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
        if (objFromDb != null)
        {
            objFromDb.Name = obj.Name;
            objFromDb.Price = obj.Price;
            objFromDb.Description = obj.Description;
            objFromDb.CategoryId = obj.CategoryId;
            objFromDb.DigiPixCoverId = obj.DigiPixCoverId;
            if (obj.ImageUrl != null)
            {
                objFromDb.ImageUrl = obj.ImageUrl;
            }
        }
    }
}
