namespace e_commerce_platform.Models;


public class SliderEditModel
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ImageName { get; set; }

    public IFormFile? ImageFolder { get; set; }

    public int Index { get; set; } //slider sırası

    public bool isActive { get; set; }
}