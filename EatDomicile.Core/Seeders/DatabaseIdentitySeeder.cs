using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EatDomicile.Core.Context;

namespace EatDomicile.Core.Seeders;

public static class DatabaseIdentitySeeder
{
    // Liste des rôles que tu veux créer
    private static readonly string[] Roles = new[] { "Admin", "User" };

    public static async Task SeedDevDataAsync(
        IdentityContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await context.Database.MigrateAsync();

        // Création des rôles si nécessaire
        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (!result.Succeeded)
                    throw new Exception($"Failed creating role'{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        if (context.Users.Any())
            return;

        var aUser = new IdentityUser
        {
            Email = "aidentity@mysite.local",
            UserName = "aidentity@mysite.local",
        };

        var createAUserResult = await userManager.CreateAsync(aUser, "StrongPassw0rd!");
        if (!createAUserResult.Succeeded)
        {
            throw new Exception($"❌ User creation error : {string.Join(", ", createAUserResult.Errors.Select(e => e.Description))}");
        }

        var roleAUserResult = await userManager.AddToRoleAsync(aUser, "Admin");
        if (!roleAUserResult.Succeeded)
        {
            throw new Exception($"❌ Role assignment error 'Admin' à aidentity@mysite.local: {string.Join(", ", roleAUserResult.Errors.Select(e => e.Description))}");
        }

        Console.WriteLine($"✅ User 'aidentity@mysite.local' creating with role 'Admin'");

        var uUser = new IdentityUser
        {
            Email = "uidentity@mysite.local",
            UserName = "uidentity@mysite.local",
        };

        var createUUserResult = await userManager.CreateAsync(uUser, "StrongPassw0rd!");
        if (!createUUserResult.Succeeded)
        {
            throw new Exception($"❌ User creation error : {string.Join(", ", createUUserResult.Errors.Select(e => e.Description))}");
        }

        var roleUUserResult = await userManager.AddToRoleAsync(uUser, "User");
        if (!roleUUserResult.Succeeded)
        {
            throw new Exception($"❌ Role assignment error 'User' à uidentity@mysite.local: {string.Join(", ", roleUUserResult.Errors.Select(e => e.Description))}");
        }

        Console.WriteLine($"✅ User 'uidentity@mysite.local' creating with role 'User'");
    }
}
