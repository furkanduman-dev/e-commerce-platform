namespace e_commerce_platform.Models;

public class Category{

    public int Id { get; set; }

    public string Name { get; set; }

    public string Url {get; set;}

    public bool IsPopular { get; set; }

    public List<Product> Product{get; set;}
}