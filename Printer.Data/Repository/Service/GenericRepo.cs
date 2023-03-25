using Printer.Data;
using Printer.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Printer.Data.Repository.Service;

public class GenericRepo<T> : IGenericRepo<T> where T : class
{
    private readonly RepositoryContext _db;
    internal DbSet<T> dbSet;

    public GenericRepo(RepositoryContext db)
    {
        _db = db;
        //_db.ShoppingCarts.Include(u => u.Product).Include(u=>u.DigiPixCover);
        this.dbSet = _db.Set<T>();
    }
    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    //includeProp - "Category,DigiPixCover"
    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        return query.ToList();
    }

    public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
    {
        if (tracked)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }
        else
        {
            IQueryable<T> query = dbSet.AsNoTracking();

            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }
}
