using EatDomicile.Web.Services.Users;
using EatDomicile.Web.Services.Users.DTO;
using EatDomicile.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Web.Controllers;

public class UsersController : Controller
{
    private readonly UsersService usersService;

    public UsersController(UsersService usersService)
    {
        this.usersService = usersService;
    }


    // GET: UsersController
    public async Task<IActionResult> Index()
    {
        var users = await this.usersService.GetUsersAsync();
        return View(users.Select(static user => new UsersViewModel()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Mail = user.Mail
        }));
    }

    // GET: UsersController/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var user = await this.usersService.GetUserAsync(id);

        if (user is null)
        {
            return this.NotFound();
        }

        UsersDTO userDto = new UsersDTO()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Mail = user.Mail
        };

        var ingredients = await this.usersService.GetUserAddress(id);

        return this.View(new UsersDetailsViewModel()
        {
            User = userDto,
            Ingredients = ingredients
        });
    }


    public IActionResult Create()
    {
        var userCreateViewModel = new UsersCreateViewModel();
        return this.View(userCreateViewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind("Name,Price,Vegetarian")] UserCreateViewModel userCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(userCreateViewModel);

        try
        {
            var newUser = new UsersDTO
            {
                Name = userCreateViewModel.Name,
                Price = userCreateViewModel.Price,
                Vegetarian = userCreateViewModel.Vegetarian
            };

            await this.usersService.CreateUserAsync(newUser);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View();
        }
    }


    public async Task<ActionResult> Edit(int id)
    {
        var user = await this.userService.GetUserAsync(id);

        if (user is null)
        {
            return this.NotFound();
        }

        var userEditViewModel = new UserEditViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Price = user.Price,
            Vegetarian = user.Vegetarian,
        };

        return this.View(userEditViewModel);
    }

    // POST: UsersController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, [Bind("Id,Name,Price,Vegetarian")] UserEditViewModel userEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(userEditViewModel);

        try
        {
            var editUser = new UsersDTO
            {
                Id = userEditViewModel.Id,
                Name = userEditViewModel.Name,
                Price = userEditViewModel.Price,
                Vegetarian = userEditViewModel.Vegetarian
            };

            await this.userService.UpdateUserAsync(id, editUser);

            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View();
        }
    }

    public async Task<ActionResult> Delete(int id)
    {
        var user = await this.userService.GetUserAsync(id);

        if (user is null)
        {
            return this.NotFound();
        }

        var userFound = new UserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Price = user.Price,
            Vegetarian = user.Vegetarian
        };

        return this.View(userFound);
    }

    // POST: DrinksController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await this.userService.DeleteUserAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View(nameof(this.Details));
        }
    }


    [HttpGet("Users/{id}/Ingredients/Add")]
    public async Task<IActionResult> AddIngredient(int id)
    {
        var user = await this.userService.GetUserAsync(id);

        this.ViewData["userSelected"] = user.Name;
        this.ViewData["userSelectedId"] = user.Id;

        return this.View(new IngredientCreateViewModel());
    }

    // POST: UsersController/Edit/5
    [HttpPost("Users/{id}/Ingredients/Add")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> AddIngredient(int id, [Bind("Name,KCal,IsAllergen")] IngredientCreateViewModel ingredientCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(ingredientCreateViewModel);

        try
        {
            var newIngredient = new IngredientDTO
            {
                Name = ingredientCreateViewModel.Name,
                KCal = ingredientCreateViewModel.KCal,
                IsAllergen = ingredientCreateViewModel.IsAllergen
            };

            await this.userService.UpdateUserAddIngredientAsync(id, newIngredient);

            return this.RedirectToAction(nameof(this.Details), new { id = id });
        }
        catch
        {
            return this.View();
        }
    }

    [HttpGet("Users/{userId}/Ingredients/{id}/Edit")]
    public async Task<IActionResult> EditIngredient(int userId, int id)
    {
        var user = await this.userService.GetUserAsync(userId);

        this.ViewData["userSelected"] = user.Name;
        this.ViewData["userSelectedId"] = user.Id;
        var ingredient = await this.ingredientService.GetIngredientAsync(id);

        var ingredientEditViewModel = new IngredientEditViewModel
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            KCal = ingredient.KCal,
            IsAllergen = ingredient.IsAllergen,
        };

        return this.View(ingredientEditViewModel);
    }

    // POST: UsersController/Edit/5
    [HttpPost("Users/{userId}/Ingredients/{id}/Edit")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> EditIngredient(int userId, int id, [Bind("Id,Name,KCal,IsAllergen")] IngredientEditViewModel ingredientEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(ingredientEditViewModel);

        try
        {
            var editIngredient = new IngredientDTO
            {
                Id = ingredientEditViewModel.Id,
                Name = ingredientEditViewModel.Name,
                KCal = ingredientEditViewModel.KCal,
                IsAllergen = ingredientEditViewModel.IsAllergen
            };

            await this.userService.UpdateUserUpdateIngredientAsync(userId, id, editIngredient);

            return this.RedirectToAction(nameof(this.Details), new { id = userId });
        }
        catch
        {
            return this.View();
        }
    }

    [HttpGet("Users/{userId}/Ingredients/{id}/Delete")]
    public async Task<ActionResult> DeleteIngredient(int userId, int id)
    {
        var user = await this.userService.GetUserAsync(userId);

        this.ViewData["userSelected"] = user.Name;
        this.ViewData["userSelectedId"] = user.Id;
        var ingredient = await this.ingredientService.GetIngredientAsync(id);

        var ingredientEditViewModel = new UserIngredientViewModel
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            KCal = ingredient.KCal,
            IsAllergen = ingredient.IsAllergen,
            UserId = userId
        };

        return this.View(ingredientEditViewModel);
    }

    // POST: DrinksController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteIngredientConfirmed(int userId, int id)
    {
        try
        {
            await this.userService.UpdateUserDeleteIngredientAsync(userId, id);
            return this.RedirectToAction(nameof(this.Details), new { id = userId });
        }
        catch
        {
            return this.RedirectToAction(nameof(this.Details), new { id = userId });
        }
    }
}