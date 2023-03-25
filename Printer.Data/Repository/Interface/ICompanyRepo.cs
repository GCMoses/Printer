using Printer.Core.Models;
using Printer.Data.Repository.Interface;

public interface ICompanyRepo : IGenericRepo<Company>
{
    void Update(Company obj);
}
