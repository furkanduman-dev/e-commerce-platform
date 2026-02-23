namespace e_commerce_platform.Models;


public class UserEditModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }

    public IList<string>? SelectedRoles { get; set; }
}