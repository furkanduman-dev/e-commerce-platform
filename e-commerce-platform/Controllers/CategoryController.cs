using System.Security.Cryptography.X509Certificates;
using e_commerce_platform.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace e_commerce_platform.Controllers;


public class CategoryController : Controller
{

    private readonly DataContext _context;

    public CategoryController(DataContext context)
    {
      _context = context;  
    }



    public ActionResult Index(CategoryGetModel model)
    {
        var categoryies = _context.Categories.Select(i=>new CategoryGetModel
        {
            Name=i.Name,
             Url=i.Url,
              IsPopular=i.IsPopular,
               ProductCount=i.Product.Count,
               Id=i.Id,
                Image=i.Image
             
               
        }).ToList();

        return View(categoryies);
    }

    public ActionResult Create ()
    {
        return View();
    }
   
    [HttpPost] 
     public async Task<ActionResult> CreateAsync (CategoryCreateModel addmodel)
    {
        if(ModelState.IsValid)
        {
              var fileName = Path.GetRandomFileName() + ".jpg";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

            using (var stream = new FileStream(path, FileMode.Create)){
            await  addmodel.Image!.CopyToAsync(stream);
        }
            var addcategory = new Category
            {
                Name=addmodel.Name,
                IsPopular=addmodel.IsPopular,
                Url=addmodel.Url,
                Image=fileName
                
            };
            _context.Categories.Add(addcategory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(addmodel);
    }

    public ActionResult Edit(int id)
    {
        var catedit = _context.Categories.Select(i=> new CategoryEditModel{

             Id=i.Id,
              IsPopular=i.IsPopular,
               Name=i.Name,
                Url=i.Url,
                ImageName=i.Image
            
        }).FirstOrDefault(i=>i.Id==id);
        
        return View(catedit);
    }
    
    [HttpPost] 
    public async Task<ActionResult> EditAsync(int id, CategoryEditModel editmodel)
    {
        if( id != editmodel.Id)
        {
            NotFound();
        }

        if(ModelState.IsValid)
        {
            var edit =_context.Categories.FirstOrDefault(i=>i.Id==editmodel.Id);
            
            if(edit !=null)
            {
                    var fileName=Path.GetRandomFileName() + ".jpg";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img",fileName);

                    using (var stream = new FileStream(path, FileMode.Create)){
                        await editmodel.ImageFolder!.CopyToAsync(stream);
                    }

                    edit.Image=fileName;
             }
                 edit.Name=editmodel.Name;
                 edit.IsPopular=editmodel.IsPopular;
                 edit.Url=editmodel.Url;

                 _context.SaveChanges();

                 return RedirectToAction("Index");
                
            }

             return View(editmodel);
    
                
    }

       

    public ActionResult Delete (int ? id)
    {
        if(id==null)
        {
            return NotFound();
        }

        var catdelete =_context.Categories.FirstOrDefault(i=>i.Id==id);

        if(catdelete != null)
        {
            _context.Categories.Remove(catdelete);
            _context.SaveChanges();
            TempData["Mesaj"]=$"{catdelete.Name} kategorisi silindi";
        }
         return RedirectToAction("Index");
    }
}