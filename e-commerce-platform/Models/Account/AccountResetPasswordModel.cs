using System.ComponentModel.DataAnnotations;

namespace e_commerce_platform.Models;


public class AccountResetPasswordModel
{

    public string Token { get; set; } = null!;

    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;
}