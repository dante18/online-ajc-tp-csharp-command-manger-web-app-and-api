using EatDomicile.Core.Entities;
using EatDomicile.Web.Services.Doughs;
using EatDomicile.Web.Services.Pizzas;
using EatDomicile.Web.Services.Pizzas.DTO;
using EatDomicile.Web.ViewModels.Doughs;
using EatDomicile.Web.ViewModels.Pizzas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EatDomicile.Web.Controllers;

public class PizzasController : Controller
{
    private readonly PizzasService pizzasService;
    private readonly DoughsService doughService;

    public PizzasController(PizzasService pizzasService, DoughsService doughService)
    {
        this.pizzasService = pizzasService;
        this.doughService = doughService;
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
        var pizza = await this.pizzasService.GetPizzaAsync(id);
        if (pizza is null)
        {
            return this.NotFound();
        }

        return this.View(new PizzaDetailsViewModel()
        {
            Id = pizza.Id,
            Name = pizza.Name,
            Price = pizza.Price,
            Doughs = pizza.Doughs,
            Vegetarian = pizza.Vegetarian
        });
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
            return this.View();
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        var pizza = await this.pizzasService.GetPizzaAsync(id);
        if (pizza is null)
        {
            return this.NotFound();
        }

        var pizzaEditViewModel = new PizzaEditViewModel
        {
            Name = pizza.Name,
            Price = pizza.Price,
            Doughs = pizza.Doughs,
            Vegetarian = pizza.Vegetarian
        };

        return this.View(pizzaEditViewModel);
    }

    // POST PizzasController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, [Bind("Id,Name,Price,Fizzy,KCal")] PizzaEditViewModel pizzaEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(pizzaEditViewModel);

        try
        {
            var editPizza = new PizzaDTO()
            {
                Name = pizzaEditViewModel.Name,
                Price = pizzaEditViewModel.Price,
                Doughs = pizzaEditViewModel.Doughs,
                Vegetarian = pizzaEditViewModel.Vegetarian
            };

            await this.pizzasService.UpdatePizzaAsync(id, editPizza);

            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View();
        }
    }

    public async Task<ActionResult> Delete(int id)
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

    // POST: Pizza/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await this.pizzasService.GetPizzaAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View(nameof(this.Details));
        }
    }
}
