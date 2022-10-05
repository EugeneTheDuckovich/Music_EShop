using Microsoft.AspNetCore.Authentication;

namespace eshop.ViewsModel;

public class LoginViewModel
{
    public string? UserName { get; set; }

    public string Password { get; set; }

    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }
    public IList<AuthenticationScheme>? ExternalLogins { get; set; }
}