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

    public async Task TransferCartToUser(string UserName)
    {
        var userCart = await GetCart(UserName);
    }
}