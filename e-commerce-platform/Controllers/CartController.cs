using e_commerce_platform.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_platform.Controllers;


public class CartController : Controller
{

    private readonly DataContext _context;


    public CartController(DataContext context)
    {
        _context = context;
    }

    public ActionResult Index()
    {

        return View();
    }
}