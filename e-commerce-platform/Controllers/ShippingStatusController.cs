using e_commerce_platform.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_platform.Controllers;


public class ShippingStatusController : Controller
{

    private readonly DataContext _context;

    public ShippingStatusController(DataContext context)
    {
        _context = context;
    }

    public ActionResult Index()
    {
        var ship = _context.ShippingStatuses.ToList();

        return View(ship);
    }

    public ActionResult Create()
    {
        return View();
    }



    [HttpPost]
    public ActionResult Create(ShippingCreateStatus createmodel)
    {
        if (ModelState.IsValid)
        {
            var status = new ShippingStatus
            {
                Name = createmodel.Name,
                Status = createmodel.Status,
                Step = createmodel.Step
            };

            _context.ShippingStatuses.Add(status);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(createmodel);
    }

    public ActionResult Edit(int id)
    {

        var edit = _context.ShippingStatuses.Select(i => new ShippingEditStatus
        {
            Id = i.Id,
            Name = i.Name,
            Status = i.Status,
            Step = i.Step
        }).FirstOrDefault(i => i.Id == id);

        return View(edit);
    }

    [HttpPost]
    public ActionResult Edit(int id, ShippingEditStatus shippingEdit)
    {

        if (id != shippingEdit.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var editmodel = _context.ShippingStatuses.FirstOrDefault(i => i.Id == shippingEdit.Id);

            editmodel.Id = shippingEdit.Id;
            editmodel.Name = shippingEdit.Name;
            editmodel.Status = shippingEdit.Status;
            editmodel.Step = shippingEdit.Step;

            _context.SaveChanges();

            return RedirectToAction("Index");

        }

        return View(shippingEdit);
    }

    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var statusdelete = _context.ShippingStatuses.FirstOrDefault(i => i.Id == id);

        if (statusdelete != null)
        {
            _context.ShippingStatuses.Remove(statusdelete);
            _context.SaveChanges();
            TempData["Mesaj"] = $"{statusdelete.Name} Statusu silindi";
        }
        return RedirectToAction("Index");
    }

}