using System.Threading.Tasks;
using e_commerce_platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace e_commerce_platform.Controllers;


public class UserController : Controller
{

    private UserManager<AppUser> _userManager;

    private RoleManager<AppRole> _roleManager;
    private readonly DataContext _context;

    public UserController(DataContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _context = context;
        _roleManager = roleManager;
    }

    public async Task<ActionResult> Index(string role)
    {
        ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name", role);

        if (!string.IsNullOrEmpty(role))
        {
            return View(await _userManager.GetUsersInRoleAsync(role));
        }

        return View(_userManager.Users);
    }
}