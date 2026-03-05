using System.Threading.Tasks;
using e_commerce_platform.Models;
using e_commerce_platform.Services;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace e_commerce_platform.Controllers;


public class OrderController : Controller
{

    private readonly DataContext _context;

    private readonly ICartService _cartService;

    private readonly IConfiguration _configuration;

    public OrderController(DataContext context, ICartService cartService, IConfiguration configuration)
    {
        _cartService = cartService;
        _configuration = configuration;
        _context = context;
    }

    public ActionResult Index()
    {
        return View();
    }

    public ActionResult Details()
    {
        return View();
    }

    public async Task<ActionResult> Checkout()
    {
        ViewBag.Cart = await _cartService.GetCart(User.Identity?.Name!);
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> Checkout(OrderCreateModel createModel)
    {
        var userName = User.Identity?.Name!;
        var cart = await _cartService.GetCart(userName);

        if (cart.CartItems.Count == 0)
        {
            ModelState.AddModelError("", "Sepetinizde Ürün Bulunamamıştır");
        }
        if (ModelState.IsValid)
        {
            var order = new Order
            {
                AdresSatırı = createModel.AdresSatiri,
                Adsoyad = createModel.AdSoyad,
                Telefon = createModel.Telefon,
                PostaKodu = createModel.PostaKodu,
                Sehir = createModel.Sehir,
                SiparisNotu = createModel.SiparisNotu,
                SiparisTarihi = DateTime.Now,
                ToplamFiyat = cart.Toplam(),
                Username = userName,
                OrderItems = cart.CartItems.Select(ci => new Models.OrderItem
                {
                    ProductId = ci.ProductId,
                    Fiyat = ci.Product.Price,
                    Miktar = ci.Miktar
                }).ToList()
            };

            var payment = await ProcessPayment(createModel, cart);

            if (payment.Status == "success")
            {
                _context.Add(order);
                _context.Remove(cart);

                await _context.SaveChangesAsync();

                return RedirectToAction("Complated", new { orderId = order.Id });
            }
            else
            {
                ModelState.AddModelError("", payment.ErrorMessage);
            }

        }
        ViewBag.cart = cart;

        return View(createModel);
    }

    public ActionResult Complated(string orderId)
    {
        return View("Complated", orderId);
    }

    public async Task<ActionResult> OrderList()
    {
        var username = User.Identity?.Name;
        var orders = await _context.Orders
        .Include(i => i.OrderItems)
        .ThenInclude(i => i.product)
        .Where(i => i.Username == username)
        .ToListAsync();

        return View(orders);
    }

    private async Task<Payment> ProcessPayment(OrderCreateModel model, Cart cart)
    {
        Iyzipay.Options options = new Iyzipay.Options();
        options.ApiKey = _configuration["PaymentAPI:APIKey"];
        options.SecretKey = _configuration["PaymentAPI:SecretKey"]; ;
        options.BaseUrl = "https://sandbox-api.iyzipay.com";

        CreatePaymentRequest request = new CreatePaymentRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = Guid.NewGuid().ToString();//benzersiz bir veri
        request.Price = cart.AraToplam().ToString();
        request.PaidPrice = cart.AraToplam().ToString();
        request.Currency = Currency.TRY.ToString();
        request.Installment = 1;
        request.BasketId = "B67832";
        request.PaymentChannel = PaymentChannel.WEB.ToString();
        request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

        //karttan alınan bilgiler

        PaymentCard paymentCard = new PaymentCard();
        paymentCard.CardHolderName = model.CartName;
        paymentCard.CardNumber = model.CartNumber;
        paymentCard.ExpireMonth = model.CartExpirationMonth;
        paymentCard.ExpireYear = model.CartExpirationYear;
        paymentCard.Cvc = model.CartCVV;
        paymentCard.RegisterCard = 0;
        request.PaymentCard = paymentCard;

        Buyer buyer = new Buyer();
        buyer.Id = "BY789";
        buyer.Name = model.AdSoyad;
        buyer.Surname = "DOE";
        buyer.GsmNumber = model.Telefon;
        buyer.Email = "email@email.com";
        buyer.IdentityNumber = "74300864791";
        buyer.LastLoginDate = "2015-10-05 12:43:35";
        buyer.RegistrationDate = "2013-04-21 15:12:09";
        buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
        buyer.Ip = "85.34.78.112";
        buyer.City = model.Sehir;
        buyer.Country = "Turkey";
        buyer.ZipCode = model.PostaKodu;
        request.Buyer = buyer;

        Address shippingAddress = new Address();
        shippingAddress.ContactName = model.AdSoyad;
        shippingAddress.City = model.Sehir;
        shippingAddress.Country = "Turkey";
        shippingAddress.Description = model.AdresSatiri;
        shippingAddress.ZipCode = model.PostaKodu;
        request.ShippingAddress = shippingAddress;

        Address billingAddress = new Address();
        billingAddress.ContactName = model.AdSoyad;
        billingAddress.City = model.PostaKodu;
        billingAddress.Country = "Turkey";
        billingAddress.Description = model.AdresSatiri;
        billingAddress.ZipCode = model.PostaKodu;
        request.BillingAddress = billingAddress;

        List<BasketItem> basketItems = new List<BasketItem>();

        foreach (var item in cart.CartItems)
        {
            BasketItem BasketItem = new BasketItem();
            BasketItem.Id = item.CartId.ToString();
            BasketItem.Name = item.Product.Name;
            BasketItem.Category1 = "Collectibles";
            BasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            BasketItem.Price = item.Product.Price.ToString();

            basketItems.Add(BasketItem);
        }

        request.BasketItems = basketItems;

        // BasketItem secondBasketItem = new BasketItem();
        // secondBasketItem.Id = "BI102";
        // secondBasketItem.Name = "Game code";
        // secondBasketItem.Category1 = "Game";
        // secondBasketItem.Category2 = "Online Game Items";
        // secondBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
        // secondBasketItem.Price = "0.5";
        // basketItems.Add(secondBasketItem);

        // BasketItem thirdBasketItem = new BasketItem();
        // thirdBasketItem.Id = "BI103";
        // thirdBasketItem.Name = "Usb";
        // thirdBasketItem.Category1 = "Electronics";
        // thirdBasketItem.Category2 = "Usb / Cable";
        // thirdBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
        // thirdBasketItem.Price = "0.2";
        // basketItems.Add(thirdBasketItem);
        // request.BasketItems = basketItems;

        return await Payment.Create(request, options);
    }
}