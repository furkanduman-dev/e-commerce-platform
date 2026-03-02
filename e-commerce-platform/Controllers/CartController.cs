using System.Threading.Tasks;
using e_commerce_platform.Models;
using e_commerce_platform.Services;
using Microsoft.AspNetCore.Mvc;
using e_commerce_platform.Services;

namespace e_commerce_platform.Controllers;


public class CartController : Controller
{

    private readonly DataContext _context;
    private readonly ICartService _cartService;


    public CartController(DataContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    public async Task<ActionResult> Index()
    {

        var customerId = _cartService.GetCustomerId();
        var cart = await _cartService.GetCart(customerId);


        return View(cart);
    }

    [HttpPost]
    public async Task<ActionResult> AddToCart(int productId, int Miktar = 1)
    {
        await _cartService.AddToCart(productId, Miktar);

        return RedirectToAction("Index", "Cart");
    }
    [HttpPost]
    public async Task<ActionResult> RemoveItem(int productId, int Miktar)
    {
        await _cartService.RemoveItem(productId, Miktar);

        return RedirectToAction("Index", "Cart");
    }
}