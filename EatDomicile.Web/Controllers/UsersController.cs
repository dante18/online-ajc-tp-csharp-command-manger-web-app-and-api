using EatDomicile.Web.Services.Domains.Addresses.DTO;
using EatDomicile.Web.Services.Domains.Users.DTO;
using EatDomicile.Web.Services.Interfaces;
using EatDomicile.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Web.Controllers;

public class UsersController : Controller
{
    private readonly IApiUsersService usersService;

    public UsersController(IApiUsersService usersService)
    {
        this.usersService = usersService;
    }


    // GET: UsersController
    public async Task<IActionResult> Index()
    {
        var users = await this.usersService.GetUsersAsync();

        return View(users.Select(static user => new UsersViewModel()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Mail = user.Mail
        }));
    }

    // GET: UsersController/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var user = await this.usersService.GetUserAsync(id);

            if (user is null)
            {
                return this.NotFound();
            }

            UserDTO userDto = new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Mail = user.Mail,
                Address = new AddressDTO()
                {
                    Id = user.Address.Id,
                    Street = user.Address.Street,
                    City = user.Address.City,
                    State = user.Address.State,
                    Zip = user.Address.Zip,
                    Country = user.Address.Country
                }
            };

            return this.View(new UsersDetailsViewModel()
            {
                User = userDto,
                Address = userDto.Address
            });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "L'utilisateur n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Create()
    {
        var userCreateViewModel = new UsersCreateViewModel();

        return this.View(userCreateViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind("FirstName,LastName,Mail,Phone,Street,City,State,Zip,Country")] UsersCreateViewModel userCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(userCreateViewModel);

        try
        {
            var newUser = new UserDTO
            {
                FirstName = userCreateViewModel.FirstName,
                LastName = userCreateViewModel.LastName,
                Phone = userCreateViewModel.Phone,
                Mail = userCreateViewModel.Mail,
                Address = new AddressDTO()
                {
                    City = userCreateViewModel.City,
                    Country = userCreateViewModel.Country,
                    State = userCreateViewModel.State,
                    Zip = userCreateViewModel.Zip,
                    Street = userCreateViewModel.Street
                }
            };

            await this.usersService.CreateUserAsync(newUser);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la création de l'utilisateur";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        try
        {
            var user = await this.usersService.GetUserAsync(id);

            if (user is null)
            {
                return this.NotFound();
            }

            var userEditViewModel = new UsersEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Mail = user.Mail,
                Street = user.Address.Street,
                City = user.Address.City,
                State = user.Address.State,
                Zip = user.Address.Zip,
                Country = user.Address.Country
            };

            return this.View(userEditViewModel);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "L'utilisateur n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: UsersController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Mail,Phone,Street,City,State,Zip,Country")] UsersEditViewModel userEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(userEditViewModel);

        try
        {
            var editUser = new UserDTO
            {
                FirstName = userEditViewModel.FirstName,
                LastName = userEditViewModel.LastName,
                Phone = userEditViewModel.Phone,
                Mail = userEditViewModel.Mail,
                Address = new AddressDTO()
                {
                    City = userEditViewModel.City,
                    Country = userEditViewModel.Country,
                    State = userEditViewModel.State,
                    Zip = userEditViewModel.Zip,
                    Street = userEditViewModel.Street
                }
            };

            await this.usersService.UpdateUserAsync(id, editUser);

            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la modification de l'utilisateur";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var user = await this.usersService.GetUserAsync(id);

            if (user is null)
            {
                return this.NotFound();
            }

            UserDTO userDto = new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Mail = user.Mail,
                Address = new AddressDTO()
                {
                    Id = user.Address.Id,
                    Street = user.Address.Street,
                    City = user.Address.City,
                    State = user.Address.State,
                    Zip = user.Address.Zip,
                    Country = user.Address.Country
                }
            };

            return this.View(new UsersDetailsViewModel()
            {
                User = userDto,
                Address = userDto.Address
            });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "L'utilisateur n'existe pas ou a été supprimé!";
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
            await this.usersService.DeleteUserAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la suppréssion de l'utilisateur";
            return RedirectToAction(nameof(Index));
        }
    }
}