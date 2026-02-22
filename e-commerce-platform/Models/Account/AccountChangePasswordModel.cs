using System.ComponentModel.DataAnnotations;

namespace e_commerce_platform.Models;


public class AccountChangePasswordModel {
    
    [DataType(DataType.Password)]
    public string OldPassword { get; set; } = null!;
    
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;
}