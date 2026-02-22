namespace e_commerce_platform.Models;


public class AccountLoginModel
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; } = true;
}