using Microsoft.AspNetCore.Mvc;

namespace e_commerce_platform.Controllers;


public class OrderController : Controller
{

    public ActionResult Index()
    {
        return View();
    }

    public ActionResult Details()
    {
        return View();
    }

    public ActionResult Checkout()
    {
        return View();
    }

    public ActionResult Complated()
    {
        return View();
    }
}