using e_commerce_platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_platform.Controllers;


public class ProductController : Controller
{

    private readonly DataContext _contex;

    public ProductController(DataContext context)
    {
        _contex = context;
    }

    public ActionResult Index()
    {
        var productGet = _contex.Products
            .Select(i => new ProductGetModel
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                Description = i.Description,
                IsActive = i.IsActive,
                IsHomepage = i.IsHomepage,
                Size = i.Size,
                CategoryId = i.CategoryId,
                Category = i.Category,

                MainImage = i.Images
                    .Where(x => x.IsMain)
                    .Select(x => x.ImageUrl)
                    .FirstOrDefault()
            })
            .ToList();

        return View(productGet);
    }
    public ActionResult Create()
    {
        ViewBag.Categories = new SelectList(_contex.Categories, "Id", "Name");
        return View();


    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(_contex.Categories, "Id", "Name");
            return View(model);
        }

        // 1️⃣ Ürünü oluştur
        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            IsActive = model.IsActive,
            IsHomepage = model.IsHomepage,
            Size = model.Size,
            CategoryId = model.CategoryId
        };

        _contex.Products.Add(product);
        await _contex.SaveChangesAsync(); // Id oluşur

        // 2️⃣ Resimleri kaydet
        if (model.ImageFiles != null && model.ImageFiles.Any())
        {
            int order = 0;

            foreach (var file in model.ImageFiles)
            {
                // Dosya uzantısını al
                var extension = Path.GetExtension(file.FileName);

                // Güvenli dosya adı oluştur
                var fileName = Guid.NewGuid().ToString() + extension;

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");

                // Klasör yoksa oluştur
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var productImage = new ProductImage
                {
                    ImageUrl = fileName,
                    ProductId = product.Id,
                    IsMain = order == 0,      // 👈 İlk sıradaki ana görsel
                    DisplayOrder = order
                };

                _contex.ProductImages.Add(productImage);

                order++;
            }

            await _contex.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var product = _contex.Products
            .Include(p => p.Images)
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
            return NotFound();

        var model = new ProductEditModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            IsActive = product.IsActive,
            IsHomepage = product.IsHomepage,
            Size = product.Size,
            CategoryId = product.CategoryId,
            ExistingImages = product.Images.ToList()
        };

        ViewBag.Categories = new SelectList(_contex.Categories, "Id", "Name", product.CategoryId);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductEditModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(_contex.Categories, "Id", "Name", model.CategoryId);
            return View(model);
        }

        var product = _contex.Products
            .Include(p => p.Images)
            .FirstOrDefault(p => p.Id == model.Id);

        if (product == null)
            return NotFound();

        // Güncelle
        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.IsActive = model.IsActive;
        product.IsHomepage = model.IsHomepage;
        product.Size = model.Size;
        product.CategoryId = model.CategoryId;

        // Yeni resim ekleme
        if (model.NewImages != null && model.NewImages.Any())
        {
            int order = product.Images.Count;

            foreach (var file in model.NewImages)
            {
                var extension = Path.GetExtension(file.FileName);
                var fileName = Guid.NewGuid() + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                product.Images.Add(new ProductImage
                {
                    ImageUrl = fileName,
                    IsMain = false,
                    DisplayOrder = order++
                });
            }
        }

        await _contex.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteImage(int id, int productId)
    {
        var image = await _contex.ProductImages.FindAsync(id);

        if (image == null)
            return NotFound();

        // Fiziksel dosyayı sil
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", image.ImageUrl);

        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }

        _contex.ProductImages.Remove(image);
        await _contex.SaveChangesAsync();

        return RedirectToAction("Edit", new { id = productId });
    }

    public async Task<IActionResult> SetMainImage(int id, int productId)
    {
        var images = _contex.ProductImages
            .Where(x => x.ProductId == productId)
            .ToList();

        foreach (var img in images)
        {
            img.IsMain = false;
        }

        var selectedImage = images.FirstOrDefault(x => x.Id == id);

        if (selectedImage != null)
        {
            selectedImage.IsMain = true;
        }

        await _contex.SaveChangesAsync();

        return RedirectToAction("Edit", new { id = productId });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _contex.Products
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        // Fiziksel resimleri sil
        foreach (var image in product.Images)
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/img",
                image.ImageUrl
            );

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        _contex.Products.Remove(product);
        await _contex.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}