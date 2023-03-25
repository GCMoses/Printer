using Printer.Core.Models;

namespace Printer.Data.Repository.Interface;

public interface IDigiPixCoverRepo : IGenericRepo<DigiPixCover>
{
    void Update(DigiPixCover obj);
}