using System.ComponentModel.DataAnnotations;
using e_commerce_platform.Models;

namespace e_commerce_platform.Models;


public class ProductCreateModel
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }


    public double Price { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsHomepage { get; set; } = false;

    public string? Size { get; set; }

    public int CategoryId { get; set; }

    // 👇 Çoklu upload için
    public List<IFormFile>? ImageFiles { get; set; }
}