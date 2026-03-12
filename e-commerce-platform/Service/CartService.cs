using e_commerce_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_platform.Services;


public interface ICartService
{

    string GetCustomerId();

    Task<Cart> GetCart(string customerId);

    Task AddToCart(int productId, int Miktar = 1);

    Task RemoveItem(int productId, int Miktar = 1);

    Task TransferCartToUser(string UserName);
}

public class CartService : ICartService
{
    public readonly DataContext _context;

    public readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddToCart(int productId, int Miktar = 1)
    {
        var cart = await GetCart(GetCustomerId());

        var product = await _context.Products.FirstOrDefaultAsync(i => i.Id == productId);

        if (product != null)
        {
            cart.AddItem(product, Miktar);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Cart> GetCart(string custId)
    {
        var cart = await _context.Carts
        .Include(i => i.CartItems)
        .ThenInclude(i => i.Product)
        .ThenInclude(i => i.Images)
        .Where(i => i.CustomerId == custId)
        .FirstOrDefaultAsync();

        if (cart == null)
        {
            var customerId = _httpContextAccessor.HttpContext?.User.Identity?.Name;

            if (string.IsNullOrEmpty(customerId))
            {
                customerId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1),
                    IsEssential = true
                };

                _httpContextAccessor.HttpContext?.Response.Cookies.Append("customerId", customerId, cookieOptions);
            }
            cart = new Cart { CustomerId = customerId };
            _context.Carts.Add(cart);

        }
        return cart;
    }

    public string GetCustomerId()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.User.Identity?.Name ?? context?.Request.Cookies["customerId"];
    }

    public async Task RemoveItem(int productId, int Miktar = 1)
    {
        var cart = await GetCart(GetCustomerId());
        var product = await _context.Products.FirstOrDefaultAsync(i => i.Id == productId);

        if (product != null)
        {
            cart.DeleteItem(productId, Miktar);
            await _context.SaveChangesAsync();
        }
    }

    public async Task TransferCartToUser(string UserName) //giriş yapmadan önce sepete kelnen ürünü giriş yaptıktan sonraki sepete ekler
    {
        var userCart = await GetCart(UserName); //giriş yapmış kullanıcının sepeti


        var cookieCart = await GetCart(_httpContextAccessor.HttpContext?.Request.Cookies["customerId"]!); //giriş yapmamış kullanıcın sepeti

        foreach (var item in cookieCart?.CartItems!)
        {
            var cartItem = userCart?.CartItems.Where(i => i.ProductId == item.ProductId).FirstOrDefault(); //daha önce bu ürün zaten giriş yapan kullanıcının sepetinde var mı ?

            if (cartItem != null) //varsa miktarı arttır
            {
                cartItem.Miktar += item.Miktar;
            }
            else //yoksa ekle
            {
                userCart?.CartItems.Add(new CartItem { ProductId = item.ProductId, Miktar = item.Miktar });
            }
        }
        _context.Carts.Remove(cookieCart); // login kartına aktarıldıktan sonra cookie kartını sil
        await _context.SaveChangesAsync();
    }
}