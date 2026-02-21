using System.Threading.Tasks;
using e_commerce_platform.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_platform.Controllers;


public class SliderController : Controller
{

    private readonly DataContext _context;


    public SliderController(DataContext context)
    {
        _context = context;
    }


    public ActionResult Index()
    {

        var SlidersGet = _context.Sliders.Select(i => new SliderGetModel
        {
            Id = i.Id,
            Description = i.Description,
            Image = i.Image,
            Index = i.Index,
            isActive = i.isActive,
            Title = i.Title
        }).ToList();

        return View(SlidersGet);
    }


    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(SliderCreateModel addmodel)
    {

        if (ModelState.IsValid)
        {

            var fileName = Path.GetRandomFileName() + ".jpg";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await addmodel.Image!.CopyToAsync(stream);
            }

            var addSlider = new Slider
            {
                Description = addmodel.Description,
                Index = addmodel.Index,
                isActive = addmodel.isActive,
                Title = addmodel.Title,
                Image = fileName

            };

            _context.Sliders.Add(addSlider);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(addmodel);
    }


    public ActionResult Edit(int id)
    {
        var sliderEdit = _context.Sliders.Select(i => new SliderEditModel
        {
            Description = i.Description,
            Id = i.Id,
            ImageName = i.Image,
            Index = i.Index,
            isActive = i.isActive,
            Title = i.Title

        }).FirstOrDefault(i => i.Id == id);

        return View(sliderEdit);


    }

    [HttpPost]
    public async Task<ActionResult> Edit(int id, SliderEditModel editmodel)
    {
        if (id != editmodel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var edit = _context.Sliders.FirstOrDefault(i => i.Id == editmodel.Id);

            if (edit != null)
            {
                // 🔹 Eğer yeni resim seçildiyse
                if (editmodel.ImageFolder != null && editmodel.ImageFolder.Length > 0)
                {
                    // Eski resmi sil (isteğe bağlı ama önerilir)
                    if (!string.IsNullOrEmpty(edit.Image))
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", edit.Image);

                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    var extension = Path.GetExtension(editmodel.ImageFolder.FileName);
                    var fileName = Path.GetRandomFileName() + extension;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await editmodel.ImageFolder.CopyToAsync(stream);
                    }

                    edit.Image = fileName;
                }

                // 🔹 Diğer alanları güncelle
                edit.Description = editmodel.Description;
                edit.Index = editmodel.Index;
                edit.isActive = editmodel.isActive;
                edit.Title = editmodel.Title;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        return View(editmodel);
    }

    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sliderdelete = _context.Sliders.FirstOrDefault(i => i.Id == id);

        if (sliderdelete != null)
        {
            _context.Sliders.Remove(sliderdelete);
            _context.SaveChanges();
            TempData["Mesaj"] = $"{sliderdelete.Title} kategorisi silindi";
        }
        return RedirectToAction("Index");
    }

}