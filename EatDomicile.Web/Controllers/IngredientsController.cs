using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Ingredients;
using EatDomicile.Web.ViewsModels.Ingredients;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize()]
    public async Task<IActionResult> Index(int page = 1)
    {
        var response = await this.ingredientService.GetIngredientsAsync(page);
        IngredientListViewModel model = new IngredientListViewModel()
        {
            Items = response.Items.Select(ingredient => new IngredientViewModel()
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                KCal = ingredient.KCal,
                IsAllergen = ingredient.IsAllergen
            }).ToList(),
            currentPage = response.currentPage,
            maxPage = response.maxPage,
            nextPage = response.nextPage,
            previousPage = response.previousPage
        };

        return View(model);
    }

    // GET: IngredientsController/Details/5
    [Authorize()]
    public async Task<IActionResult> Details(int id)
    {
        try
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
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "L'ingrédient n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        var ingredientCreateViewModel = new IngredientCreateViewModel();
        return this.View(ingredientCreateViewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
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
            TempData["ErrorMessage"] = "Echec de la création de l'ingrédient";
            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Edit(int id)
    {
        try
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
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "L'ingrédient n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: IngredientsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
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
            TempData["ErrorMessage"] = "Echec de la modification de l'ingrédient";
            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        try
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
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "L'ingrédient n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: DrinksController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await this.ingredientService.DeleteIngredientAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la suppréssion de l'ingrédient";
            return RedirectToAction(nameof(Index));
        }
    }
}