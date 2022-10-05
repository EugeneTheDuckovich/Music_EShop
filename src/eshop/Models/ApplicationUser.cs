using Microsoft.AspNetCore.Identity;

namespace eshop.Models;

// Add profile data for application users by adding properties to the eshopUser class
public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }

    public DateTime DOB { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Gender { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

}