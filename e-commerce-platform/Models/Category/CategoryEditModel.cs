namespace e_commerce_platform.Models;



public class CategoryEditModel
{
     public int Id { get; set; }

    public string Name { get; set; }

    public string Url {get; set;}

    public bool IsPopular { get; set; }

     public string? ImageName { get; set; }

    public IFormFile ImageFolder { get; set; }
    
}