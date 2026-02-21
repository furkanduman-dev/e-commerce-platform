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
        _context = context;
    }

    public IActionResult Index()
    {
        ViewData["Categories"] = _context.Categories.ToList();
        ViewData["Sliders"] = _context.Sliders.ToList();
        var productGet = _context.Products
            .Select(i => new ProductGetModel
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                Description = i.Description,
                IsActive = i.IsActive,
                IsHomepage = i.IsHomepage,
                Size = i.Size,
                CategoryId = i.CategoryId,
                Category = i.Category,

                MainImage = i.Images
                    .Where(x => x.IsMain)
                    .Select(x => x.ImageUrl)
                    .FirstOrDefault()
            })
            .ToList();

        return View(productGet);

    }


}

