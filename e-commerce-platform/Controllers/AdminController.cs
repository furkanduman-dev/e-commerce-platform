using e_commerce_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_platform.Controllers;


public class AdminController : Controller
{
    private readonly DataContext _context;

    public AdminController(DataContext context)
    {
        _context = context;
    }
    public ActionResult Index()
    {
        var orders = _context.Orders
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
        .ToList();

        ViewBag.ProductCount = _context.Products.Count();
        ViewBag.TotalPrice = _context.Orders.Sum(o => o.ToplamFiyat);
        ViewBag.TotalOrder = _context.Orders.Count();
        ViewBag.TotallUser = _context.Users.Count();

        return View(orders);

    }
}