using Printer.Core.Models;
using Printer.Data;
using Printer.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Printer.Data.Repository.Service;

public class DigiPixCoverRepo : GenericRepo<DigiPixCover>, IDigiPixCoverRepo
{
    private RepositoryContext _db;

    public DigiPixCoverRepo(RepositoryContext db) : base(db)
    {
        _db = db;
    }


    public void Update(DigiPixCover obj)
    {
        _db.DigiPixCovers.Update(obj);
    }
}