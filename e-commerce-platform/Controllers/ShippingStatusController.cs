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
}