namespace e_commerce_platform.Models;


public class UserGetModel
{

    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;
}