using System.Security.Claims;
using eshop.Models;
using eshop.ViewsModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eshop.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                Email = model.Email, UserName = model.Email, PhoneNumber = model.PhoneNumber, Address = model.Address,
                DOB = model.DOB, Gender = model.Gender, Name = model.Name
            };
            // добавляем пользователя
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // установка куки
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }


    public async Task<IActionResult> Login(string returnUrl)
    {
        var loginViewModel = new LoginViewModel
        {
            ReturnUrl = returnUrl,
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        };
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        return View(loginViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
    {
        model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid Login Attempt!!");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


    [HttpPost]
    public IActionResult ExternalLogin(string provider, string returnUrl)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public IActionResult ExteralLoginView()
    {
        return View();
    }

    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        var loginViewModel = new LoginViewModel
        {
            //ReturnUrl = returnUrl,
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        };
        if (remoteError != null)
        {
            ModelState.AddModelError("", $"Error from external provider:{remoteError}");
            return View("Login", loginViewModel);
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ModelState.AddModelError("", "Error loading external login information");
            return View("Login", loginViewModel);
        }

        var result =
            await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (email != null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    //Name = info.Principal.FindFirstValue(ClaimTypes.Name)
                };
                await _userManager.CreateAsync(user);
                //await _signInManager.SignInAsync(user, isPersistent: false);

                //return LocalRedirect(returnUrl);
            }

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect(returnUrl);
        }

        ViewBag.ErorTitle = $"Email claim not recieved from:{info.LoginProvider}";
        ViewBag.ErrorMessage = "Please contact support on help@gmail.com";
        return View("Error");

        return View("Login", loginViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLoginCallback(ApplicationUser model)
    {
        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            var ErrorMessage = "Error loading external login information during confirmation.";
        }

        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        return View(model);
    }
}