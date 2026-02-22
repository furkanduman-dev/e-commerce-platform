using System.ComponentModel.DataAnnotations;

namespace e_commerce_platform.Models;


public class AccountCreateModel
{

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Parola Eşleşmiyor")]
    public string ConfirmPassword { get; set; } = null!;
}