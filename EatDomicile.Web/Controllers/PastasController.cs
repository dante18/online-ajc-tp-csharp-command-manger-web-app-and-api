using EatDomicile.Web.Services.Domains.Pastas.DTO;
using EatDomicile.Web.Services.Interfaces;
using EatDomicile.Web.ViewModels.Pastas;
using Microsoft.AspNetCore.Mvc;

public class PastasController : Controller
{
    private readonly IApiPastasService pastasService;

    public PastasController(IApiPastasService pastasService)
    {
        this.pastasService = pastasService;
    }

    //GET PastasController
    public async Task<IActionResult> Index()
    {
        var pastas = await this.pastasService.GetPastasAsync();
        return View(pastas.Select(static pasta => new PastaViewModel()
        {
            Id = pasta.Id,
            Name = pasta.Name,
            Price = pasta.Price,
            Vegetarian = pasta.Vegetarian,
            KCal = pasta.KCal,
            Type = pasta.Type,
        }));
    }

    //GET PastasController/Details/id
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var pasta = await this.pastasService.GetPastaAsync(id);

            if (pasta is null)
            {
                return this.NotFound();
            }

            return this.View(new PastaDetailsViewModel()
            {
                Id = pasta.Id,
                Name = pasta.Name,
                Price = pasta.Price,
                Type = pasta.Type,
                KCal = pasta.KCal,
                Vegetarian = pasta.Vegetarian,
            });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La pasta n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Create()
    {
        var pastaCreateViewModel = new PastaCreateViewModel();
        return this.View(pastaCreateViewModel);
    }

    // POST: PastasController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind("Name,Price,Type,KCal,Vegetarian")] PastaCreateViewModel pastaCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(pastaCreateViewModel);

        try
        {
            var newPasta = new PastaDTO
            {
                Name = pastaCreateViewModel.Name,
                Price = pastaCreateViewModel.Price,
                Type = pastaCreateViewModel.Type,
                KCal = pastaCreateViewModel.KCal,
                Vegetarian = pastaCreateViewModel.Vegetarian,
            };

            await this.pastasService.CreatePastaAsync(newPasta);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la création de la pasta";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        try
        {
            var pasta = await this.pastasService.GetPastaAsync(id);

            if (pasta is null)
            {
                return this.NotFound();
            }

            var pastaEditViewModel = new PastaEditViewModel
            {
                Id = pasta.Id,
                Name = pasta.Name,
                Price = pasta.Price,
                Type = pasta.Type,
                KCal = pasta.KCal,
                Vegetarian = pasta.Vegetarian,
            };

            return this.View(pastaEditViewModel);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La pasta n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: PastasController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, [Bind("Id,Name,Price,Type,KCal,Vegetarian")] PastaEditViewModel pastaEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(pastaEditViewModel);

        try
        {
            var editPasta = new PastaDTO
            {
                Id = pastaEditViewModel.Id,
                Name = pastaEditViewModel.Name,
                Price = pastaEditViewModel.Price,
                Type = pastaEditViewModel.Type,
                KCal = pastaEditViewModel.KCal,
                Vegetarian = pastaEditViewModel.Vegetarian,
            };

            await this.pastasService.UpdatePastaAsync(id, editPasta);

            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la modification de la pasta";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var pasta = await this.pastasService.GetPastaAsync(id);

            if (pasta is null)
            {
                return this.NotFound();
            }

            var pastaFound = new PastaViewModel
            {
                Id = pasta.Id,
                Name = pasta.Name,
                Price = pasta.Price,
                Type = pasta.Type,
                KCal = pasta.KCal,
                Vegetarian = pasta.Vegetarian,
            };

            return this.View(pastaFound);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La pasta n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: PastasController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await this.pastasService.DeletePastaAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la suppression de la pasta";
            return RedirectToAction(nameof(Index));
        }
    }
}
