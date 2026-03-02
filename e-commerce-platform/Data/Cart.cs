namespace e_commerce_platform.Models;


public class Cart
{

    public int CartId { get; set; }

    public string CustomerId { get; set; }

    public List<CartItem> CartItems { get; set; } = new();

    public void AddItem(Product product, int Miktar)
    {
        var item = CartItems.Where(i => i.ProductId == product.Id).FirstOrDefault();

        if (item == null)
        {
            CartItems.Add(new CartItem
            {
                Product = product,
                Miktar = Miktar
            });
        }
        else
        {
            item.Miktar += Miktar;
        }
    }

    public void DeleteItem(int productId, int miktar)
    {
        var item = CartItems.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            item.Miktar -= miktar;

            if (item.Miktar <= 0)
            {
                CartItems.Remove(item);
            }
        }
    }

    public double AraToplam()
    {
        return CartItems.Sum(i => i.Product.Price * i.Miktar);
    }

    public double Toplam()
    {

        return CartItems.Sum(i => i.Product.Price * i.Miktar) * 1.2;
    }
}


public class CartItem
{

    public int CartItemId { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public int CartId { get; set; }

    public Cart Cart { get; set; } = null!;

    public int Miktar { get; set; }
}