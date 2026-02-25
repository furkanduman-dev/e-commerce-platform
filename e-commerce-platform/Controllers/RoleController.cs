using System.Threading.Tasks;
using e_commerce_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_platform.Controllers;


public class RoleController : Controller
{

    private RoleManager<AppRole> _roleManager;
    private UserManager<AppUser> _userManager;

    private readonly DataContext _context;

    public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, DataContext context)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }



    public ActionResult Index()
    {
        return View(_roleManager.Roles);
    }


    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(RoleCreateModel createModel)
    {
        if (ModelState.IsValid)
        {
            var role = new AppRole
            {
                Name = createModel.RoleName
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Role");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(createModel);
    }



    public async Task<ActionResult> Edit(string id)
    {

        var entity = await _roleManager.FindByIdAsync(id);

        if (entity != null)
        {
            return View(new RoleEditModel { Id = entity.Id, RoleName = entity.Name! });
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<ActionResult> Edit(string id, RoleEditModel editModel)
    {
        if (ModelState.IsValid)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity != null)
            {
                entity.Name = editModel.RoleName;

                var result = await _roleManager.UpdateAsync(entity);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }

        return View(editModel);
    }



    public async Task<ActionResult> DeleteAsync(string? id)
    {
        if (id == null)
        {

            return RedirectToAction("Index");
        }

        var entity = await _roleManager.FindByIdAsync(id);

        if (entity != null)
        {

            await _roleManager.DeleteAsync(entity);


            return RedirectToAction("Index");

            // TempData["Mesaj"]=$"{entity.Name} rolü silindi";
        }


        return RedirectToAction("Index");
    }
}



