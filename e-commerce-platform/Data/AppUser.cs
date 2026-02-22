
using Microsoft.AspNetCore.Identity;

namespace e_commerce_platform.Models;


public class AppUser : IdentityUser<int>
{
    public string FullName { get; set; } = null!;
}