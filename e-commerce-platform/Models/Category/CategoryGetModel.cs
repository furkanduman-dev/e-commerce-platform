namespace e_commerce_platform.Models;



public class CategoryGetModel
{
     public int Id { get; set; }

    public string Name { get; set; }

    public string Url {get; set;}

    public bool IsPopular { get; set; }

    public int ProductCount  {get; set;}

       public string Image { get; set; }
}