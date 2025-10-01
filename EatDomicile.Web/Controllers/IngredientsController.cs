using EatDomicile.Web.Services.Ingredient.DTO;
using EatDomicile.Web.Services.Ingredients;
using EatDomicile.Web.ViewModels.Drinks;
using EatDomicile.Web.ViewModels.Ingredients;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Web.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly IngredientsService ingredientService;

        public IngredientsController(IngredientsService ingredientService)
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

        // GET: IngredientsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name,KCal,IsAllergen")] IngredientCreateViewModel ingredientCreateViewModel)
        {
            if (!this.ModelState.IsValid)
                return this.View(ingredientCreateViewModel);

            try
            {
                var newDrink = new IngredientDTO
                {
                    Name = ingredientCreateViewModel.Name,
                    KCal = ingredientCreateViewModel.KCal,
                    IsAllergen = ingredientCreateViewModel.IsAllergen
                };

                await this.ingredientService.CreateDrinkAsync(newDrink);
                return this.RedirectToAction(nameof(this.Index));
            }
            catch
            {
                return this.View();
            }
        }

        // POST: IngredientsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: IngredientsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: IngredientsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: IngredientsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IngredientsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
