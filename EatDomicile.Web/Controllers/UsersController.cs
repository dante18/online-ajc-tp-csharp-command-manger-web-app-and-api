using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Addresses;
using EatDomicile.Web.Services.Dtos.Application.Users;
using EatDomicile.Web.ViewsModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EatDomicile.Web.Controllers;

public class UsersController : Controller
{
    private readonly IApiUsersService usersService;

    public UsersController(IApiUsersService usersService)
    {
        this.usersService = usersService;
    }


    // GET: UsersController
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index(int page = 1)
    {
        var response = await this.usersService.GetUsersAsync(page);
        UserListViewModel model = new UserListViewModel()
        {
            Items = response.Items.Select(user => new UsersViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Mail = user.Mail,
                Role = user.Role
            }).ToList(),
            currentPage = response.currentPage,
            maxPage = response.maxPage,
            nextPage = response.nextPage,
            previousPage = response.previousPage
        };

        return View(model);
    }

    // GET: UsersController/Details/5
    [Authorize(Roles = "Admin")]
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
                Role = user.Role,
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

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        List<SelectListItem> roles = new List<SelectListItem>();
        roles.Add(new SelectListItem("Utilisateur", "User"));
        roles.Add(new SelectListItem("Administrateur", "Admin"));

        var userCreateViewModel = new UsersCreateViewModel()
        {
            Roles = roles
        };

        return this.View(userCreateViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Create([Bind("FirstName,LastName,Mail,Phone,Street,City,State,Zip,Country, Password, RoleId")] UsersCreateViewModel userCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(userCreateViewModel);

        try
        {
            var newUser = new CreateUserDTO
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
                },
                Password = userCreateViewModel.Password,
                Role = userCreateViewModel.RoleId
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

    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Edit(int id)
    {
        try
        {
            var user = await this.usersService.GetUserAsync(id);

            if (user is null)
            {
                return this.NotFound();
            }

            List<SelectListItem> roles = new List<SelectListItem>();
            roles.Add(new SelectListItem("Utilisateur", "User"));
            roles.Add(new SelectListItem("Administrateur", "Admin"));

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
                Country = user.Address.Country,
                Roles = roles,
                RoleId = user.Role
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
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Mail,Phone,Street,City,State,Zip,Country, RoleId")] UsersEditViewModel userEditViewModel)
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
                Role = userEditViewModel.RoleId,
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

    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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