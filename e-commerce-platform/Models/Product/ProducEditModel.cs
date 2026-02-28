using System.ComponentModel.DataAnnotations;
using e_commerce_platform.Models;

namespace e_commerce_platform.Models;


public class ProductEditModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }


    public double Price { get; set; }

    public bool IsActive { get; set; }
    public bool IsHomepage { get; set; }

    public string? Size { get; set; }

    public int CategoryId { get; set; }

    // 🔥 Mevcut resimler (gösterme için)
    public List<ProductImage> ExistingImages { get; set; } = new();

    // 🔥 Yeni yüklenecek resimler
    public List<IFormFile>? NewImages { get; set; }
}