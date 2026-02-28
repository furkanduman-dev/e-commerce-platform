using Microsoft.AspNetCore.Mvc;

namespace e_commerce_platform.Controllers;


public class CartController : Controller
{

    public ActionResult Index()
    {

        return View();
    }
}