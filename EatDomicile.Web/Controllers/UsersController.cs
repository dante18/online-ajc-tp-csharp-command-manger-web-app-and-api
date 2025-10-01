using EatDomicile.Web.Services.Users;
using EatDomicile.Web.Services.Users;
using EatDomicile.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersService usersService;

        public UsersController(UsersService userssService)
        {
            this.usersService = usersService;
        }

        //GET UserssController
        public async Task<IActionResult> Index()
        {
            var userss = await this.usersService.GetUsersAsync();
            return View(userss.Select(static users => new UsersViewModel()
            {
                Id = users.Id,
                Name = users.Name,
                Price = users.Price,
                Fizzy = users.Fizzy,
                KCal = users.KCal,
            }));
        }

        //GET UserssController/Details/id
        public async Task<IActionResult> Details(int id)
        {
            var users = await this.usersService.GetUsersAsync(id);

            if (users is null)
            {
                return this.NotFound();
            }

            return this.View(new UsersDetailsViewModel()
            {
                Id = users.Id,
                Name = users.Name,
                Price = users.Price,
                Fizzy = users.Fizzy,
                KCal = users.KCal,
            });
        }

        public IActionResult Create()
        {
            var usersCreateViewModel = new UsersCreateViewModel();
            return this.View(usersCreateViewModel);
        }

        // POST: UserssController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name,Price,Fizzy,KCal")] UsersCreateViewModel usersCreateViewModel)
        {
            if (!this.ModelState.IsValid)
                return this.View(usersCreateViewModel);

            try
            {
                var newUsers = new UsersDTO
                {
                    Name = usersCreateViewModel.Name,
                    Price = usersCreateViewModel.Price,
                    Fizzy = usersCreateViewModel.Fizzy,
                    KCal = usersCreateViewModel.KCal,
                };

                await this.usersService.CreateUsersAsync(newUsers);
                return this.RedirectToAction(nameof(this.Index));
            }
            catch
            {
                return this.View();
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var users = await this.usersService.GetUsersAsync(id);

            if (users is null)
            {
                return this.NotFound();
            }

            var usersEditViewModel = new UsersEditViewModel
            {
                Id = users.Id,
                Name = users.Name,
                Price = users.Price,
                Fizzy = users.Fizzy,
                KCal = users.KCal,
            };

            return this.View(usersEditViewModel);
        }

        // POST: UserssController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name,Price,Fizzy,KCal")] UsersEditViewModel usersEditViewModel)
        {
            if (!this.ModelState.IsValid)
                return this.View(usersEditViewModel);

            try
            {
                var editUsers = new UsersDTO
                {
                    Id = usersEditViewModel.Id,
                    Name = usersEditViewModel.Name,
                    Price = usersEditViewModel.Price,
                    Fizzy = usersEditViewModel.Fizzy,
                    KCal = usersEditViewModel.KCal,
                };

                await this.usersService.UpdateUsersAsync(id, editUsers);

                return this.RedirectToAction(nameof(this.Index));
            }
            catch
            {
                return this.View();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            var users = await this.usersService.GetUsersAsync(id);

            if (users is null)
            {
                return this.NotFound();
            }

            var usersFound = new UsersViewModel
            {
                Id = users.Id,
                Name = users.Name,
                Price = users.Price,
                Fizzy = users.Fizzy,
                KCal = users.KCal,
            };

            return this.View(usersFound);
        }

        // POST: UserssController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await this.usersService.DeleteUsersAsync(id);
                return this.RedirectToAction(nameof(this.Index));
            }
            catch
            {
                return this.View(nameof(this.Details));
            }
        }
    }
}
