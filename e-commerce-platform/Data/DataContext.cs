namespace e_commerce_platform.Models;
using Microsoft.EntityFrameworkCore;


public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

    public DbSet<Slider> Sliders {get; set;}




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

           modelBuilder.Entity<Category>().HasData(
            new List<Category>
            {
                new Category(){ Id=1 , Name="deneme kategori" , Url="deneme" , IsPopular=true}
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new List<Product>
            {
                new Product(){ Id=1 , CategoryId=1, Description="deneme açıklama", IsActive=true , IsHomepage=true , Name="deneme ürün" , Price=1, Size="orta" }
            }
        );
      
         // PRODUCT IMAGES
        modelBuilder.Entity<ProductImage>().HasData(
            new ProductImage
            {
                Id = 1,
                ProductId = 1,
                ImageUrl = "https://blog.obilet.com/wp-content/uploads/2023/07/0anagorsel-1-scaled.jpeg",
                IsMain = true,
                DisplayOrder = 1
            }
        );
          modelBuilder.Entity<Slider>().HasData(
            new List<Slider>
            {
                new Slider(){ Id=1 ,  Description="deneme slider" , Index=0 , isActive=true , Title="Deneme slider title" , Image=""}
            }
        );
        


    }
}