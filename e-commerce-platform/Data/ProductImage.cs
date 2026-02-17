using System.ComponentModel.DataAnnotations;

namespace e_commerce_platform.Models;


public class ProductImage
{
    public int Id { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    public bool IsMain { get; set; } = false;

    public int DisplayOrder { get; set; } = 0;

    // Foreign Key
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}