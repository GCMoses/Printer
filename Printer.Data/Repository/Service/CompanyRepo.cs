using Printer.Core.Models;
using Printer.Data;
using Printer.Data.Repository.Service;

namespace Printer.Data.Repository.Service;

public class CompanyRepo : GenericRepo<Company>, ICompanyRepo
{
    private RepositoryContext _db;

    public CompanyRepo(RepositoryContext db) : base(db)
    {
        _db = db;
    }


    public void Update(Company obj)
    {
        _db.Companies.Update(obj);
    }
}