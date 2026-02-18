namespace e_commerce_platform.Models;



public class CategoryCreateModel
{


    public string Name { get; set; }

    public string Url {get; set;}

    public bool IsPopular { get; set; }

      public IFormFile Image { get; set; }
    
    
}