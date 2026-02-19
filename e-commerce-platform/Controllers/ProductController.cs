using Microsoft.AspNetCore.Mvc;

namespace e_commerce_platform.Controllers;


public class ProductController:Controller
{
    
    public ActionResult Index ()
    {
        return View();

    }
}