using System.ComponentModel.DataAnnotations;

namespace eshop.ViewsModel;

public class RegisterViewModel
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Name")]
    public string Name { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "DOB")]
    public DateTime DOB { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Adress")]

    public string Address { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Gender")]
    public string Gender { get; set; }

    [Required] [Display(Name = "Email")] public string Email { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "U did the mistake in your password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmpassword")]
    public string ConfirmPassword { get; set; }
}