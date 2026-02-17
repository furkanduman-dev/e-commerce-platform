using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using e_commerce_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_platform.Controllers;


public class HomeController : Controller
{
    private readonly DataContext _context;

    public HomeController(DataContext context)
    {
        _context=context;
    }

    public IActionResult Index()
    {
       ViewData ["Categories"] = _context.Categories.ToList();
       ViewData ["Sliders"] = _context.Sliders.ToList();
       var products = _context.Products
                           .Include(p => p.Images)
                           .ToList();

        return View(products);
    }

  
}

