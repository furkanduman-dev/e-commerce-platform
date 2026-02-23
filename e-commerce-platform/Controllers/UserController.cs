using Microsoft.AspNetCore.Mvc;

namespace e_commerce_platform.Controllers;


public class UserController : Controller
{

    public ActionResult Index()
    {
        return View();
    }
}