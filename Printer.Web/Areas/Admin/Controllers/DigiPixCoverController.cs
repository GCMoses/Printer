using Printer.Core.Models;
using Printer.Data.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Printer.Utilities;

namespace Printer.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Statics.Role_Admin)]
public class DigiPixCoverController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public DigiPixCoverController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<DigiPixCover> objDigiPixCoverList = _unitOfWork.DigiPixCover.GetAll();
        return View(objDigiPixCoverList);
    }

    //GET
    public IActionResult Create()
    {
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(DigiPixCover obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.DigiPixCover.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "DigiPix Cover created";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var DigiPixCoverFromDbFirst = _unitOfWork.DigiPixCover.GetFirstOrDefault(u => u.Id == id);

        if (DigiPixCoverFromDbFirst == null)
        {
            return NotFound();
        }

        return View(DigiPixCoverFromDbFirst);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(DigiPixCover obj)
    {

        if (ModelState.IsValid)
        {
            _unitOfWork.DigiPixCover.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "DigiPix Cover updated";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var DigiPixCoverFromDbFirst = _unitOfWork.DigiPixCover.GetFirstOrDefault(u => u.Id == id);

        if (DigiPixCoverFromDbFirst == null)
        {
            return NotFound();
        }

        return View(DigiPixCoverFromDbFirst);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.DigiPixCover.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.DigiPixCover.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "DigiPix Cover deleted";
        return RedirectToAction("Index");

    }
}