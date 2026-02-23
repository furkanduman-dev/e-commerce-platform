using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_platform.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{

    public ActionResult Index()
    {
        return View();

    }
}