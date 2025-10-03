using EatDomicile.Web.Services.Domains.Drinks.DTO;
using EatDomicile.Web.Services.Interfaces;
using EatDomicile.Web.ViewModels.Drinks;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Web.Controllers;

public class DrinksController : Controller
{
    private readonly IApiDrinksService drinksService;

    public DrinksController(IApiDrinksService drinksService)
    {
        this.drinksService = drinksService;
    }

    //GET DrinksController
    public async Task<IActionResult> Index()
    {
        var drinks = await this.drinksService.GetDrinksAsync();
        return View(drinks.Select(static drink => new DrinkViewModel()
        {
            Id = drink.Id,
            Name = drink.Name,
            Price = drink.Price,
            Fizzy = drink.Fizzy,
            KCal = drink.KCal,
        }));
    }

    //GET DrinksController/Details/id
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var drink = await this.drinksService.GetDrinkAsync(id);

            if (drink is null)
            {
                return this.NotFound();
            }

            return this.View(new DrinkDetailsViewModel()
            {
                Id = drink.Id,
                Name = drink.Name,
                Price = drink.Price,
                Fizzy = drink.Fizzy,
                KCal = drink.KCal,
            });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La boisson n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Create()
    {
        var drinkCreateViewModel = new DrinkCreateViewModel();
        return this.View(drinkCreateViewModel);
    }

    // POST: DrinksController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind("Name,Price,Fizzy,KCal")] DrinkCreateViewModel drinkCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(drinkCreateViewModel);

        try
        {
            var newDrink = new DrinkDTO
            {
                Name = drinkCreateViewModel.Name,
                Price = drinkCreateViewModel.Price,
                Fizzy = drinkCreateViewModel.Fizzy,
                KCal = drinkCreateViewModel.KCal,
            };

            await this.drinksService.CreateDrinkAsync(newDrink);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la création de la boisson";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        try
        {
            var drink = await this.drinksService.GetDrinkAsync(id);

            if (drink is null)
            {
                return this.NotFound();
            }

            var drinkEditViewModel = new DrinkEditViewModel
            {
                Id = drink.Id,
                Name = drink.Name,
                Price = drink.Price,
                Fizzy = drink.Fizzy,
                KCal = drink.KCal,
            };

            return this.View(drinkEditViewModel);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La boisson n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: DrinksController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, [Bind("Id,Name,Price,Fizzy,KCal")] DrinkEditViewModel drinkEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(drinkEditViewModel);

        try
        {
            var editDrink = new DrinkDTO
            {
                Id = drinkEditViewModel.Id,
                Name = drinkEditViewModel.Name,
                Price = drinkEditViewModel.Price,
                Fizzy = drinkEditViewModel.Fizzy,
                KCal = drinkEditViewModel.KCal,
            };

            await this.drinksService.UpdateDrinkAsync(id, editDrink);

            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la modification de la boisson";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var drink = await this.drinksService.GetDrinkAsync(id);

            if (drink is null)
            {
                return this.NotFound();
            }

            var drinkFound = new DrinkViewModel
            {
                Id = drink.Id,
                Name = drink.Name,
                Price = drink.Price,
                Fizzy = drink.Fizzy,
                KCal = drink.KCal,
            };

            return this.View(drinkFound);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La boisson n'existe pas ou a été supprimé!";
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
            await this.drinksService.DeleteDrinkAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la suppréssion de la boisson";
            return RedirectToAction(nameof(Index));
        }
    }
}
