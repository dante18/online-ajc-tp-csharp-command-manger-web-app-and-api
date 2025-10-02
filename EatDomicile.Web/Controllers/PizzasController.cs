using EatDomicile.Web.Services.Domains.Doughs.DTO;
using EatDomicile.Web.Services.Domains.Ingredients.DTO;
using EatDomicile.Web.Services.Domains.Pizzas.DTO;
using EatDomicile.Web.Services.Interfaces;
using EatDomicile.Web.ViewModels.Ingredients;
using EatDomicile.Web.ViewModels.Pizzas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EatDomicile.Web.Controllers;

public class PizzasController : Controller
{
    private readonly IApiPizzasService pizzasService;

    private readonly IApiDoughsService doughService;

    private readonly IApiIngredientsService ingredientService;

    public PizzasController(IApiPizzasService pizzasService, IApiDoughsService doughService, IApiIngredientsService ingredientService)
    {
        this.pizzasService = pizzasService;
        this.doughService = doughService;
        this.ingredientService = ingredientService;
    }

    // GET PizzasController
    public async Task<IActionResult> Index()
    {
        var pizza = await this.pizzasService.GetPizzasAsync();
        return View(pizza.Select(static pizza => new PizzaViewModel()
        {
            Id = pizza.Id,
            Name = pizza.Name,
            Price = pizza.Price,
            Doughs = pizza.Doughs,
            Vegetarian = pizza.Vegetarian
        }));
    }

    // GET PizzasController/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var pizza = await this.pizzasService.GetPizzaAsync(id);
            if (pizza is null)
            {
                return this.NotFound();
            }

            PizzaDTO pizzaDto = new PizzaDTO()
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Price = pizza.Price,
                Vegetarian = pizza.Vegetarian,
                Doughs = new DoughsDTO()
                {
                    Id = pizza.Doughs.Id,
                    Name = pizza.Doughs.Name
                }
            };

            var ingredients = await this.pizzasService.GetPizzaIngredientsAsync(id);

            return this.View(new PizzaDetailsViewModel()
            {
                Pizza = pizzaDto,
                Ingredients = ingredients
            });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La pizza n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Create()
    {
        var doughs = await this.doughService.GetDoughsAsync();
        var pizzaCreateViewModel = new PizzaCreateViewModel
        {
            DoughsList = doughs.Select(dough => new SelectListItem(dough.Name, dough.Id.ToString())).ToList()
        };
        return this.View(pizzaCreateViewModel);
    }

    // POST: PizzasController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind("Name,Price,DoughId,Vegetarian")] PizzaCreateViewModel pizzaCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(pizzaCreateViewModel);
        try
        {
            var newPizza = new CreatePizzaDTO
            {
                Name = pizzaCreateViewModel.Name,
                Price = pizzaCreateViewModel.Price,
                DoughsId = pizzaCreateViewModel.DoughId,
                Vegetarian = pizzaCreateViewModel.Vegetarian
            };

            await this.pizzasService.CreatePizzaAsync(newPizza);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la création de la pizza";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        try
        {
            var pizza = await this.pizzasService.GetPizzaAsync(id);
            var doughs = await this.doughService.GetDoughsAsync();

            if (pizza is null)
            {
                return this.NotFound();
            }

            var pizzaEditViewModel = new PizzaEditViewModel
            {
                Name = pizza.Name,
                Price = pizza.Price,
                DoughsList = doughs.Select(dough => new SelectListItem(dough.Name, dough.Id.ToString())).ToList(),
                DoughId = pizza.Doughs.Id,
                Vegetarian = pizza.Vegetarian
            };

            return this.View(pizzaEditViewModel);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La pizza n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST PizzasController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, [Bind("Name,Price,DoughId,Vegetarian")] PizzaEditViewModel pizzaEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(pizzaEditViewModel);

        try
        {
            var editPizza = new CreatePizzaDTO()
            {
                Name = pizzaEditViewModel.Name,
                Price = pizzaEditViewModel.Price,
                DoughsId = pizzaEditViewModel.DoughId,
                Vegetarian = pizzaEditViewModel.Vegetarian
            };

            await this.pizzasService.UpdatePizzaAsync(id, editPizza);

            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la modification de la pizza";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var pizza = await this.pizzasService.GetPizzaAsync(id);

            if (pizza is null)
            {
                return this.NotFound();
            }

            var pizzaFound = new PizzaViewModel
            {
                Name = pizza.Name,
                Price = pizza.Price,
                Doughs = pizza.Doughs,
                Vegetarian = pizza.Vegetarian
            };

            return this.View(pizzaFound);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La pizza n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Pizza/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await this.pizzasService.DeletePizzaAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la suppression de la pizza";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet("Pizzas/{id}/Ingredients/Add")]
    public async Task<IActionResult> AddIngredient(int id)
    {
        var pizza = await this.pizzasService.GetPizzaAsync(id);

        this.ViewData["pizzaSelected"] = pizza.Name;
        this.ViewData["pizzaSelectedId"] = pizza.Id;

        return this.View(new IngredientCreateViewModel());
    }

    // POST: PizzasController/Edit/5
    [HttpPost("Pizzas/{id}/Ingredients/Add")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> AddIngredient(int id, [Bind("Name,KCal,IsAllergen")] IngredientCreateViewModel ingredientCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(ingredientCreateViewModel);

        try
        {
            var newIngredient = new IngredientDTO()
            {
                Name = ingredientCreateViewModel.Name,
                KCal = ingredientCreateViewModel.KCal,
                IsAllergen = ingredientCreateViewModel.IsAllergen
            };

            await this.pizzasService.UpdatePizzaAddIngredientAsync(id, newIngredient);

            return this.RedirectToAction(nameof(this.Details), new { id = id });
        }
        catch
        {
            return this.View();
        }
    }

    [HttpGet("Pizzas/{pizzaId}/Ingredients/{id}/Edit")]
    public async Task<IActionResult> EditIngredient(int pizzaId, int id)
    {
        var pizza = await this.pizzasService.GetPizzaAsync(pizzaId);

        this.ViewData["pizzaSelected"] = pizza.Name;
        this.ViewData["pizzaSelectedId"] = pizza.Id;
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

    // POST: PizzasController/Edit/5
    [HttpPost("Pizzas/{pizzaId}/Ingredients/{id}/Edit")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> EditIngredient(int pizzaId, int id, [Bind("Id,Name,KCal,IsAllergen")] IngredientEditViewModel ingredientEditViewModel)
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

            await this.pizzasService.UpdatePizzaUpdateIngredientAsync(pizzaId, id, editIngredient);

            return this.RedirectToAction(nameof(this.Details), new { id = pizzaId });
        }
        catch
        {
            return this.View();
        }
    }

    [HttpGet("Pizzas/{pizzaId}/Ingredients/{id}/Delete")]
    public async Task<ActionResult> DeleteIngredient(int pizzaId, int id)
    {
        var pizza = await this.pizzasService.GetPizzaAsync(pizzaId);

        this.ViewData["pizzaSelected"] = pizza.Name;
        this.ViewData["pizzaSelectedId"] = pizza.Id;
        var ingredient = await this.ingredientService.GetIngredientAsync(id);

        var ingredientEditViewModel = new PizzaIngredientViewModel
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            KCal = ingredient.KCal,
            IsAllergen = ingredient.IsAllergen,
            PizzaId = pizzaId
        };

        return this.View(ingredientEditViewModel);
    }

    // POST: DrinksController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteIngredientConfirmed(int pizzaId, int id)
    {
        try
        {
            await this.pizzasService.UpdatePizzaDeleteIngredientAsync(pizzaId, id);
            return this.RedirectToAction(nameof(this.Details), new { id = pizzaId });
        }
        catch
        {
            return this.RedirectToAction(nameof(this.Details), new { id = pizzaId });
        }
    }
}
