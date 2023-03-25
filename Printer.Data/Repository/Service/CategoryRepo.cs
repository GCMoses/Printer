using Printer.Core.Models;
using Printer.Data;
using Printer.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Printer.Data.Repository.Service;

public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
{
    private RepositoryContext _db;

    public CategoryRepo(RepositoryContext db) : base(db)
    {   
        _db = db;
    }


    public void Update(Category obj)
    {
        _db.Categories.Update(obj);
    }
}
