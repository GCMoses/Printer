using Printer.Core.Models;
using Printer.Data;
using Printer.Data.Repository.Interface;
using Printer.Data.Repository.Service;

namespace Printer.Data.Repository.Service;

public class AppUserRepo : GenericRepo<AppUser>, IAppUserRepo
{
    private RepositoryContext _db;

    public AppUserRepo(RepositoryContext db) : base(db)
    {
        _db = db;
    }

}