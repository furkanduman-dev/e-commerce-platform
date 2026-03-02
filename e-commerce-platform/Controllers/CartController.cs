using System.Threading.Tasks;
using e_commerce_platform.Models;
using e_commerce_platform.Service;
using Microsoft.AspNetCore.Mvc;
using e_commerce_platform.Service;

namespace e_commerce_platform.Controllers;


public class CartController : Controller
{

    private readonly DataContext _context;
    private readonly ICartService _cartservice;


    public CartController(DataContext context, ICartService cartService)
    {
        _context = context;
        _cartservice = cartService;
    }

    public async Task<ActionResult> Index()
    {

        var customerId = _cartservice.GetCustomerId();
        var cart = await _cartservice.GetCart(customerId);

        return View(cart);
    }

    [HttpPost]
    public async Task<ActionResult> AddToCart(int productId, int Miktar = 1)
    {
        await _cartservice.AddToCart(productId, Miktar);

        return RedirectToAction("Index", "Cart");
    }
    [HttpPost]
    public async Task<ActionResult> RemoveItem(int productId, int Miktar)
    {
        await _cartservice.RemoveItem(productId, Miktar);

        return RedirectToAction("Index", "Cart");
    }
}