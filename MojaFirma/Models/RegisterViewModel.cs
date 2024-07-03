using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Potwierdź hasło")]
    [Compare("Password", ErrorMessage = "Hasła nie są zgodne.")]
    public string ConfirmPassword { get; set; }

    [Required]
    [Display(Name = "Imię")]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Rola")]
    public string Role { get; set; }
}
