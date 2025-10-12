using System.Security.Claims;
using EatDomicile.Web.Services.Abstractions.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using EatDomicile.Web.ViewsModels.Security;

namespace EatDomicile.Web.Controllers;

public class SecurityController : Controller
{
    private readonly IAuthService authService;

    public SecurityController(IAuthService authService)
        => this.authService = authService;

    [HttpGet]
    public IActionResult Login()
        => this.View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!this.ModelState.IsValid)
            return this.View(model);

        var token = await this.authService.LoginAsync(new()
        {
            Email = model.Email,
            Password = model.Password,
            RememberMe = model.RememberMe,
        });

        if (token != null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, model.Email),
                new("Token", token),
            };

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            claims.AddRange(jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
            };

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new(claimsIdentity),
                authProperties);

            this.Response.Cookies.Append("AccessToken", token, new CookieOptions
            {
                HttpOnly = true,              // protégé contre JavaScript
                Secure = true,                // HTTPS obligatoire
                SameSite = SameSiteMode.None, // nécessaire si API sur autre domaine/port
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return this.RedirectToAction("Index", "Home");
        }

        this.ModelState.AddModelError("", "Email ou mot de passe incorrect");
        return this.View(model);
    }

    [HttpGet]
    public IActionResult Register()
        => this.View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!this.ModelState.IsValid)
            return this.View(model);

        var success = await this.authService.RegisterAsync(new()
        {
            Email = model.Email,
            Password = model.Password,
            ConfirmPassword = model.ConfirmPassword,
        });

        if (success)
        {
            this.TempData["SuccessMessage"] = "Inscription réussie ! Vous pouvez maintenant vous connecter.";
            return this.RedirectToAction(nameof(Login));
        }

        this.ModelState.AddModelError("", "Une erreur est survenue lors de l'inscription");
        return this.View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return this.RedirectToAction("Login", "Security");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied() => View();
}