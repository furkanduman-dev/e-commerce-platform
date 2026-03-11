namespace e_commerce_platform.Models;


public class Order
{

    public int Id { get; set; }

    public DateTime SiparisTarihi { get; set; }

    public string Adsoyad { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Sehir { get; set; } = null!;

    public string AdresSatırı { get; set; } = null!;

    public string PostaKodu { get; set; } = null!;

    public string Telefon { get; set; } = null!;


    public string SiparisNotu { get; set; }

    public double ToplamFiyat { get; set; }




    // Foreign Key
    public int ShippingStatusId { get; set; }

    // Navigation
    public ShippingStatus ShippingStatus { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();





    public double AraToplam()
    {

        return OrderItems.Sum(i => i.Fiyat * i.Miktar);
    }
    public double Toplam()
    {

        return OrderItems.Sum(i => i.Fiyat * i.Miktar) * 1.2;
    }
}

public class OrderItem
{

    public int Id { get; set; }

    public int OrderId { get; set; }

    public Order Order { get; set; } = null!; //order tablosundan bir veri kullanmak için

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public double Fiyat { get; set; }

    public int Miktar { get; set; }

}