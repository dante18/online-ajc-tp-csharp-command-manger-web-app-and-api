using EatDomicile.Web.Services.Domains.Burgers.DTO;
using EatDomicile.Web.Services.Domains.Ingredients.DTO;
using EatDomicile.Web.Services.Interfaces;
using EatDomicile.Web.ViewModels.Burgers;
using EatDomicile.Web.ViewModels.Ingredients;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Web.Controllers;

public class BurgersController : Controller
{
    private readonly IApiBurgersService burgerService;

    private readonly IApiIngredientsService ingredientService;

    public BurgersController(IApiBurgersService burgerService, IApiIngredientsService ingredientService)
    {
        this.burgerService = burgerService;
        this.ingredientService = ingredientService;
    }


    // GET: BurgersController
    public async Task<IActionResult> Index()
    {
        var burgers = await this.burgerService.GetBurgersAsync();
        return View(burgers.Select(static burger => new BurgerViewModel()
        {
            Id = burger.Id,
            Name = burger.Name,
            Price = burger.Price,
            Vegetarian = burger.Vegetarian
        }));
    }

    // GET: BurgersController/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var burger = await this.burgerService.GetBurgerAsync(id);

            if (burger is null)
            {
                return this.NotFound();
            }

            BurgerDTO burgerDto = new BurgerDTO()
            {
                Id = burger.Id,
                Name = burger.Name,
                Price = burger.Price,
                Vegetarian = burger.Vegetarian
            };

            var ingredients = await this.burgerService.GetBurgerIngredientsAsync(id);

            return this.View(new BurgerDetailsViewModel()
            {
                Burger = burgerDto,
                Ingredients = ingredients
            });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Le burger n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }


    public IActionResult Create()
    {
        var burgerCreateViewModel = new BurgerCreateViewModel();
        return this.View(burgerCreateViewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind("Name,Price,Vegetarian")] BurgerCreateViewModel burgerCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(burgerCreateViewModel);

        try
        {
            var newBurger = new CreateOrUpdateBurgerDTO
            {
                Name = burgerCreateViewModel.Name,
                Price = burgerCreateViewModel.Price,
                Vegetarian = burgerCreateViewModel.Vegetarian
            };

            await this.burgerService.CreateBurgerAsync(newBurger);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la création du burger";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        try
        {
            var burger = await this.burgerService.GetBurgerAsync(id);

            if (burger is null)
            {
                return this.NotFound();
            }

            var burgerEditViewModel = new BurgerEditViewModel
            {
                Id = burger.Id,
                Name = burger.Name,
                Price = burger.Price,
                Vegetarian = burger.Vegetarian,
            };

            return this.View(burgerEditViewModel);
        }
        catch
        {
            TempData["ErrorMessage"] = "Le burger n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: BurgersController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, [Bind("Id,Name,Price,Vegetarian")] BurgerEditViewModel burgerEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(burgerEditViewModel);

        try
        {
            var editBurger = new BurgerDTO
            {
                Id = burgerEditViewModel.Id,
                Name = burgerEditViewModel.Name,
                Price = burgerEditViewModel.Price,
                Vegetarian = burgerEditViewModel.Vegetarian
            };

            await this.burgerService.UpdateBurgerAsync(id, editBurger);

            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la modification du burger";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var burger = await this.burgerService.GetBurgerAsync(id);

            if (burger is null)
            {
                return this.NotFound();
            }

            var burgerFound = new BurgerViewModel
            {
                Id = burger.Id,
                Name = burger.Name,
                Price = burger.Price,
                Vegetarian = burger.Vegetarian
            };

            return this.View(burgerFound);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Le burger n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: DrinksController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await this.burgerService.DeleteBurgerAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la suppression du burger";
            return RedirectToAction(nameof(Index));
        }
    }


    [HttpGet("Burgers/{id}/Ingredients/Add")]
    public async Task<IActionResult> AddIngredient(int id)
    {
        var burger = await this.burgerService.GetBurgerAsync(id);

        this.ViewData["burgerSelected"] = burger.Name;
        this.ViewData["burgerSelectedId"] = burger.Id;

        return this.View(new IngredientCreateViewModel());
    }

    // POST: BurgersController/Edit/5
    [HttpPost("Burgers/{id}/Ingredients/Add")]
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

            await this.burgerService.UpdateBurgerAddIngredientAsync(id, newIngredient);

            return this.RedirectToAction(nameof(this.Details), new { id = id });
        }
        catch
        {
            return this.View();
        }
    }

    [HttpGet("Burgers/{burgerId}/Ingredients/{id}/Edit")]
    public async Task<IActionResult> EditIngredient(int burgerId, int id)
    {
        var burger = await this.burgerService.GetBurgerAsync(burgerId);

        this.ViewData["burgerSelected"] = burger.Name;
        this.ViewData["burgerSelectedId"] = burger.Id;
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

    // POST: BurgersController/Edit/5
    [HttpPost("Burgers/{burgerId}/Ingredients/{id}/Edit")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> EditIngredient(int burgerId, int id, [Bind("Id,Name,KCal,IsAllergen")] IngredientEditViewModel ingredientEditViewModel)
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

            await this.burgerService.UpdateBurgerUpdateIngredientAsync(burgerId, id, editIngredient);

            return this.RedirectToAction(nameof(this.Details), new { id = burgerId });
        }
        catch
        {
            return this.View();
        }
    }

    [HttpGet("Burgers/{burgerId}/Ingredients/{id}/Delete")]
    public async Task<ActionResult> DeleteIngredient(int burgerId, int id)
    {
        var burger = await this.burgerService.GetBurgerAsync(burgerId);

        this.ViewData["burgerSelected"] = burger.Name;
        this.ViewData["burgerSelectedId"] = burger.Id;
        var ingredient = await this.ingredientService.GetIngredientAsync(id);

        var ingredientEditViewModel = new BurgerIngredientViewModel
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            KCal = ingredient.KCal,
            IsAllergen = ingredient.IsAllergen,
            BurgerId = burgerId
        };

        return this.View(ingredientEditViewModel);
    }

    // POST: DrinksController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteIngredientConfirmed(int burgerId, int id)
    {
        try
        {
            await this.burgerService.UpdateBurgerDeleteIngredientAsync(burgerId, id);
            return this.RedirectToAction(nameof(this.Details), new { id = burgerId });
        }
        catch
        {
            return this.RedirectToAction(nameof(this.Details), new { id = burgerId });
        }
    }
}