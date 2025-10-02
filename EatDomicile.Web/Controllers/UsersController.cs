using EatDomicile.Core.Entities;
using EatDomicile.Web.Services.Addresses;
using EatDomicile.Web.Services.Addresses.DTO;
using EatDomicile.Web.Services.Users;
using EatDomicile.Web.Services.Users.DTO;
using EatDomicile.Web.ViewModels.Addresses;
using EatDomicile.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Web.Controllers;

public class UsersController : Controller
{
    private readonly UsersService usersService;

    public UsersController(UsersService usersService)
    {
        this.usersService = usersService;
    }


    // GET: UsersController
    public async Task<IActionResult> Index()
    {
        var users = await this.usersService.GetUsersAsync();
        return View(users.Select(static user => new UsersViewModel()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Mail = user.Mail,
            Address = user.Address,
        }));
    }

    // GET: UsersController/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var user = await this.usersService.GetUserAsync(id);

        if (user is null)
        {
            return this.NotFound();
        }

        UsersDTO userDto = new UsersDTO()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Mail = user.Mail,
            Address = user.Address
        };
        return this.View(new UsersDetailsViewModel()
        {
            User = userDto,
        });
    }


    public IActionResult Create()
    {
        var userCreateViewModel = new UsersCreateViewModel();
        return this.View(userCreateViewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind("FirstName,LastName,Mail,Phone")] UsersCreateViewModel userCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(userCreateViewModel);

        try
        {
            var newUser = new UsersDTO
            {
                FirstName = userCreateViewModel.FirstName,
                LastName = userCreateViewModel.LastName,
                Phone = userCreateViewModel.Phone,
                Mail = userCreateViewModel.Mail
            };

            await this.usersService.CreateUserAsync(newUser);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View();
        }
    }


    public async Task<ActionResult> Edit(int id)
    {
        var user = await this.usersService.GetUserAsync(id);

        if (user is null)
        {
            return this.NotFound();
        }

        var userEditViewModel = new UsersEditViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Mail = user.Mail
        };

        return this.View(userEditViewModel);
    }

    // POST: UsersController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, [Bind("FirstName,LastName,Mail,Phone")] UsersEditViewModel userEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(userEditViewModel);

        try
        {
            var editUser = new UsersDTO
            {
                FirstName = userEditViewModel.FirstName,
                LastName = userEditViewModel.LastName,
                Phone = userEditViewModel.Phone,
                Mail = userEditViewModel.Mail
            };

            await this.usersService.UpdateUserAsync(id, editUser);

            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            return this.View();
        }
    }

    public async Task<ActionResult> Delete(int id)
    {
        var user = await this.usersService.GetUserAsync(id);

        if (user is null)
        {
            return this.NotFound();
        }

        var userFound = new UsersViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Mail = user.Mail
        };

        return this.View(userFound);
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
            return this.View(nameof(this.Details));
        }
    }


    [HttpGet("Users/{id}/Addresses/Add")]
    public async Task<IActionResult> AddAddress(int id)
    {
        var user = await this.usersService.GetUserAsync(id);

        this.ViewData["userSelected"] = $"{user.FirstName} {user.LastName}";
        this.ViewData["userSelectedId"] = user.Id;

        return this.View(new AddressCreateViewModel());
    }

    // POST: UsersController/Edit/5
    [HttpPost("Users/{id}/Addresss/Add")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> AddAddress(int id, [Bind("Street, City, State, Zip, Country")] AddressCreateViewModel addressCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(addressCreateViewModel);

        try
        {
            var newAddress = new AddressDTO
            {
                Street = addressCreateViewModel.Street,
                City = addressCreateViewModel.City,
                State = addressCreateViewModel.State,
                Zip = addressCreateViewModel.Zip,
                Country = addressCreateViewModel.Country,
            };

            await this.usersService.UpdateUserAddAddressAsync(id, newAddress);

            return this.RedirectToAction(nameof(this.Details), new { id = id });
        }
        catch
        {
            return this.View();
        }
    }

    [HttpGet("Users/{userId}/Addresss/{id}/Edit")]
    public async Task<IActionResult> EditAddress(int userId, int id)
    {
        var user = await this.usersService.GetUserAsync(userId);

        this.ViewData["userSelected"] = $"{user.FirstName} {user.LastName}";
        this.ViewData["userSelectedId"] = user.Id;
        var address = await this.addressService.GetAddressAsync(id);

        var addressEditViewModel = new AddressEditViewModel
        {
            Street = address.Street,
            City = address.City,
            State = address.State,
            Zip = address.Zip,
            Country = address.Country,
        };

        return this.View(addressEditViewModel);
    }

    // POST: UsersController/Edit/5
    [HttpPost("Users/{userId}/Addresss/{id}/Edit")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> EditAddress(int userId, int id, [Bind("Street, City, State, Zip, Country")] AddressEditViewModel addressEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(addressEditViewModel);

        try
        {
            var editAddress = new AddressDTO
            {
                Street = addressEditViewModel.Street,
                City = addressEditViewModel.City,
                State = addressEditViewModel.State,
                Zip = addressEditViewModel.Zip,
                Country = addressEditViewModel.Country,
            };

            await this.usersService.UpdateUserUpdateAddressAsync(userId, id, editAddress);

            return this.RedirectToAction(nameof(this.Details), new { id = userId });
        }
        catch
        {
            return this.View();
        }
    }

    [HttpGet("Users/{userId}/Addresss/{id}/Delete")]
    public async Task<ActionResult> DeleteAddress(int userId, int id)
    {
        var user = await this.usersService.GetUserAsync(userId);

        this.ViewData["userSelected"] = $"{user.FirstName} {user.LastName}";
        this.ViewData["userSelectedId"] = user.Id;
        var address = await this.addressService.GetAddressAsync(id);

        var addressEditViewModel = new UsersAddressViewModel
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City,
            State = address.State,
            Zip = address.Zip,
            Country = address.Country,
            UserId = userId
        };

        return this.View(addressEditViewModel);
    }

    // POST: DrinksController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteAddressConfirmed(int userId, int id)
    {
        try
        {
            await this.usersService.UpdateUserDeleteAddressAsync(userId, id);
            return this.RedirectToAction(nameof(this.Details), new { id = userId });
        }
        catch
        {
            return this.RedirectToAction(nameof(this.Details), new { id = userId });
        }
    }
}