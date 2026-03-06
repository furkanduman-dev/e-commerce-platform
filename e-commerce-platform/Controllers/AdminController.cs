using e_commerce_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_platform.Controllers;


public class AdminController : Controller
{
    private readonly DataContext _context;

    public AdminController(DataContext context)
    {
        _context = context;
    }
    public ActionResult Index(ProductGetModel model)
    {

        return View();

    }
}