using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using e_commerce_platform.Models;
using Microsoft.AspNetCore.Authorization;
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


    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(AccountLoginModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                await _singInManager.SignOutAsync();

                var result = await _singInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                    await _userManager.SetLockoutEndDateAsync(user, null);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    var locjoutDate = await _userManager.GetLockoutEndDateAsync(user);
                    var timeLeft = locjoutDate.Value - DateTime.UtcNow;

                    ModelState.AddModelError("", $"Hesabınız kilitlendi Lütfen {timeLeft.Minutes + 1} dakika sonra tekrar deneyiniz");

                }
                else
                {
                    ModelState.AddModelError("", "Hatalı Parola");
                }
            }
            else
            {
                ModelState.AddModelError("", "Hatalı Email");
            }
        }
        return View(model);
    }

    public async Task<ActionResult> LogOut()
    {

        await _singInManager.SignOutAsync();

        return RedirectToAction("Login", "Account");
    }

    [Authorize]
    public async Task<ActionResult> EditUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }
        return View(new AccountEditUserModel
        {
            Email = user.Email,
            FullName = user.FullName
        });


    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> EditUser(AccountEditUserModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user != null)
            {
                user.Email = model.Email;
                user.FullName = model.FullName;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Mesaj"] = "Bilgileriniz Güncellendi";
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
        }

        return View(model);

    }




}