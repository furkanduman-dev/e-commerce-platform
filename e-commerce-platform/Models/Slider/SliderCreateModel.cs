namespace e_commerce_platform.Models;


public class SliderCreateModel
{



    public string? Title { get; set; }

    public string? Description { get; set; }

    public IFormFile Image { get; set; }

    public int Index { get; set; } //slider sırası

    public bool isActive { get; set; }
}