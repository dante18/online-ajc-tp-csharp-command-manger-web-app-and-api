using EatDomicile.Web.Services.Ingredient.DTO;
using EatDomicile.Web.Services.Interfaces;
using EatDomicile.Web.ViewModels.Ingredients;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Web.Controllers;

public class IngredientsController : Controller
{
    private readonly IApiIngredientsService ingredientService;

    public IngredientsController(IApiIngredientsService ingredientService)
    {
        this.ingredientService = ingredientService;
    }

    // GET: IngredientsController
    public async Task<IActionResult> Index()
    {
        var ingredients = await this.ingredientService.GetIngredientsAsync();
        return View(ingredients.Select(static ingredient => new IngredientViewModel()
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            KCal = ingredient.KCal,
            IsAllergen = ingredient.IsAllergen
        }));
    }

    // GET: IngredientsController/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var ingredient = await this.ingredientService.GetIngredientAsync(id);

        if (ingredient is null)
        {
            return this.NotFound();
        }

        return this.View(new IngredientDetailsViewModel()
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            KCal = ingredient.KCal,
            IsAllergen = ingredient.IsAllergen
        });
    }

    public IActionResult Create()
    {
        var ingredientCreateViewModel = new IngredientCreateViewModel();
        return this.View(ingredientCreateViewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind("Name,KCal,IsAllergen")] IngredientCreateViewModel ingredientCreateViewModel)
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

            await this.ingredientService.CreateIngredientAsync(newIngredient);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View();
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        var ingredient = await this.ingredientService.GetIngredientAsync(id);

        if (ingredient is null)
        {
            return this.NotFound();
        }

        var ingredientEditViewModel = new IngredientEditViewModel
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            KCal = ingredient.KCal,
            IsAllergen = ingredient.IsAllergen,
        };

        return this.View(ingredientEditViewModel);
    }

    // POST: IngredientsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, [Bind("Id,Name,KCal,IsAllergen")] IngredientEditViewModel ingredientEditViewModel)
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

            await this.ingredientService.UpdateIngredientAsync(id, editIngredient);

            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View();
        }
    }

    public async Task<ActionResult> Delete(int id)
    {
        var ingredient = await this.ingredientService.GetIngredientAsync(id);

        if (ingredient is null)
        {
            return this.NotFound();
        }

        var ingredientFound = new IngredientViewModel
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            KCal = ingredient.KCal,
            IsAllergen = ingredient.IsAllergen
        };

        return this.View(ingredientFound);
    }

    // POST: DrinksController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await this.ingredientService.DeleteIngredientAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View(nameof(this.Details));
        }
    }
}