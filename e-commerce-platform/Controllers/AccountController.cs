using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using e_commerce_platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace e_commerce_platform.Controllers;


public class AccountController : Controller
{

    private UserManager<AppUser> _userManager;
    private SignInManager<AppUser> _singInManager;



    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _singInManager = signInManager;

    }



    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(AccountCreateModel createModel)
    {

        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                UserName = createModel.Email,
                Email = createModel.Email,
                FullName = createModel.FullName
            };

            var result = await _userManager.CreateAsync(user, createModel.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");

            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);

            }
        }
        return View(createModel);
    }

}