using EatDomicile.Web.Services.Doughs;
using EatDomicile.Web.Services.Doughs.DTO;
using EatDomicile.Web.ViewModels.Doughs;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Web.Controllers;

public class DoughsController : Controller
{
    private readonly DoughsService doughsService;

    public DoughsController(DoughsService doughsService)
    {
        this.doughsService = doughsService;
    }

    //GET DoughsController
    public async Task<IActionResult> Index()
    {
        var doughs = await this.doughsService.GetDoughsAsync();
        return View(doughs.Select(static dough => new DoughViewModel()
        {
            Id = dough.Id,
            Name = dough.Name
        }));
    }

    //GET DoughsController/Details/id
    public async Task<IActionResult> Details(int id)
    {
        var dough = await this.doughsService.GetDoughAsync(id);

        if (dough is null)
        {
            return this.NotFound();
        }

        return this.View(new DoughDetailsViewModel()
        {
            Id = dough.Id,
            Name = dough.Name
        });
    }

    public IActionResult Create()
    {
        var doughCreateViewModel = new DoughCreateViewModel();
        return this.View(doughCreateViewModel);
    }

    // POST: DoughsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind("Name")] DoughCreateViewModel doughCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(doughCreateViewModel);

        try
        {
            var newDough = new DoughsDTO
            {
                Name = doughCreateViewModel.Name,
            };

            await this.doughsService.CreateDoughAsync(newDough);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View();
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        var dough = await this.doughsService.GetDoughAsync(id);

        if (dough is null)
        {
            return this.NotFound();
        }

        var doughEditViewModel = new DoughEditViewModel
        {
            Id = dough.Id,
            Name = dough.Name
        };

        return this.View(doughEditViewModel);
    }

    // POST: DoughsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, [Bind("Id,Name,Price,Fizzy,KCal")] DoughEditViewModel doughEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(doughEditViewModel);

        try
        {
            var editDough = new DoughsDTO()
            {
                Id = doughEditViewModel.Id,
                Name = doughEditViewModel.Name
            };

            await this.doughsService.UpdateDoughAsync(id, editDough);

            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View();
        }
    }

    public async Task<ActionResult> Delete(int id)
    {
        var dough = await this.doughsService.GetDoughAsync(id);

        if (dough is null)
        {
            return this.NotFound();
        }

        var doughFound = new DoughViewModel
        {
            Id = dough.Id,
            Name = dough.Name
        };

        return this.View(doughFound);
    }

    // POST: DoughsController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await this.doughsService.DeleteDoughAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View(nameof(this.Details));
        }
    }
}
