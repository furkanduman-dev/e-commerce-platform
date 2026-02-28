using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using e_commerce_platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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



    public ActionResult Create()
    {

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(UserCreateModel createModel)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                FullName = createModel.FullName,
                UserName = createModel.Email,
                Email = createModel.Email
            };
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
        }

        return View(createModel);
    }

    public async Task<ActionResult> Edit(string id)
    {
        var entity = await _userManager.FindByIdAsync(id);

        ViewBag.Roles = await _roleManager.Roles.Select(i => i.Name).ToListAsync();

        if (entity != null)
        {
            return View(new UserEditModel
            {
                Id = entity.Id,
                Email = entity.Email,
                FullName = entity.FullName,
                SelectedRoles = await _userManager.GetRolesAsync(entity)
            });

        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<ActionResult> Edit(string id, UserEditModel editModel)
    {
        if (ModelState.IsValid)
        {
            var entityedit = await _userManager.FindByIdAsync(id);

            if (entityedit != null)
            {
                entityedit.Email = editModel.Email;
                entityedit.FullName = editModel.FullName;

                var result = await _userManager.UpdateAsync(entityedit);

                if (result.Succeeded && !string.IsNullOrEmpty(editModel.Password))
                {

                    await _userManager.RemovePasswordAsync(entityedit);
                    await _userManager.AddPasswordAsync(entityedit, editModel.Password);
                }

                if (result.Succeeded)
                {
                    await _userManager.RemoveFromRolesAsync(entityedit, await _userManager.GetRolesAsync(entityedit));

                    if (editModel.SelectedRoles != null)
                    {
                        await _userManager.AddToRolesAsync(entityedit, editModel.SelectedRoles);
                    }

                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
        }

        return View();
    }

    public async Task<ActionResult> Delete(string? id)
    {

        if (id == null)
        {

            return RedirectToAction("Index");
        }

        var entity = await _userManager.FindByIdAsync(id);

        if (entity != null)
        {

            await _userManager.DeleteAsync(entity);

            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }

}