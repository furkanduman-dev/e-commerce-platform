using e_commerce_platform.Models;
using e_commerce_platform.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IEmailService, SmtpEmailService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ICartService, CartService>();


builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString);
});

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
//login configurasyonu
builder.Services.Configure<IdentityOptions>(optins =>
{

    optins.Password.RequiredLength = 7; //şifre 7 karakterden oluşsun
    optins.Password.RequireNonAlphanumeric = false; //şifre alfanumerik zorunlu değil
    optins.Password.RequireLowercase = false; //küçük harf zorunlu değil
    optins.Password.RequireUppercase = false; ///büyük harf zorunlu değil

    optins.User.RequireUniqueEmail = true; // 1 e-mailden 1 tane hsap açılabilir

});
//Yetkilendirme
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; //default, yani her kullanıcı buraya giriş yapamaz
    options.AccessDeniedPath = "/Account/AccessDenied"; //yetkisi olmayan kişiler buraya giriş yapamaz yani herkes admin panele gidemez
    options.ExpireTimeSpan = TimeSpan.FromDays(30); // 30 gün boyunca login olan kullanıcı otomatik olarak çıkış yapmaz (saat vs olarak güncelleyebilirsin)
    options.SlidingExpiration = true;//her giriş yapıldığında expiretimespan sıfırlanır 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
