using EatDomicile.Core.Abstractions.Security;
using EatDomicile.Core.Dtos.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> logger;

    private readonly UserManager<IdentityUser> userManager;

    private readonly SignInManager<IdentityUser> signInManager;

    private readonly RoleManager<IdentityRole> roleManager;

    private readonly IJwtService jwtService;

    public AuthController(IConfiguration configuration, ILogger<AuthController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IJwtService jwtService)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        this.jwtService = jwtService;
        this.logger.LogInformation("AuthController initialized");
    }

    [HttpPost("register")]
    public async Task<IResult> Register([FromBody] RegisterModel model)
    {
        // Validate input parameters
        if (string.IsNullOrWhiteSpace(model.Email))
        {
            this.logger.LogWarning("Token request rejected: Username is empty or null");
            return Results.BadRequest(new { Error = "Username is required" });
        }

        if (string.IsNullOrWhiteSpace(model.Password))
        {
            this.logger.LogWarning("Token request rejected: Password is empty or null for username: {Username}", model.Email);
            return Results.BadRequest(new { Error = "Password is required" });
        }

        if (!this.ModelState.IsValid)
            return Results.BadRequest(this.ModelState);

        var userExists = await this.userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
            return Results.BadRequest("Un utilisateur avec cet email existe déjà");

        var user = new IdentityUser
        {
            Email = model.Email,
            UserName = model.Email,
        };

        var result = await this.userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return Results.BadRequest(result.Errors);

        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new IdentityRole("User"));

        await userManager.AddToRoleAsync(user, "User");

        return Results.Ok();
    }


    [HttpPost("login")]
    public async Task<IResult> Login(LoginModel loginModel)
    {
        // Validate input parameters
        if (string.IsNullOrWhiteSpace(loginModel.Email))
        {
            this.logger.LogWarning("Token request rejected: Username is empty or null");
            return Results.BadRequest(new { Error = "Username is required" });
        }

        if (string.IsNullOrWhiteSpace(loginModel.Password))
        {
            this.logger.LogWarning("Token request rejected: Password is empty or null for username: {Username}", loginModel.Email);
            return Results.BadRequest(new { Error = "Password is required" });
        }

        var user = await this.userManager.FindByEmailAsync(loginModel.Email);
        if (user is null)
            return Results.Unauthorized();

        var signInResult = await this.signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);

        if (!signInResult.Succeeded)
            return Results.Unauthorized();

        var token = this.jwtService.GenerateToken(user);
        var expiration = DateTime.UtcNow.AddMinutes(60);
        return Results.Ok(new AuthResponse
        {
            Email = loginModel.Email,
            Token = token,
            Expiration = expiration,
        });
    }
}
