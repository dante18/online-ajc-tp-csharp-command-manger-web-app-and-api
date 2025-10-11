using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Addresses;
using EatDomicile.Web.Services.Dtos.Application.Orders;
using EatDomicile.Web.Services.Dtos.Application.Users;
using EatDomicile.Web.ViewsModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EatDomicile.Web.Controllers;

public class OrdersController : Controller
{
    private readonly IApiOrdersService orderService;

    private readonly IApiProductsService productService;

    private readonly IApiUsersService usersService;

    public OrdersController(IApiOrdersService orderService, IApiProductsService productService, IApiUsersService usersService)
    {
        this.orderService = orderService;
        this.productService = productService;
        this.usersService = usersService;
    }


    // GET: OrdersController
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index(int page = 1)
    {
        var response = await this.orderService.GetOrdersAsync(page);
        OrderListViewModel model = new OrderListViewModel()
        {
            Items = response.Items.Select(order => new OrderViewModel()
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                Status = order.Status,
                User = new UserDTO()
                {
                    Id = order.User.Id,
                    FirstName = order.User.FirstName,
                    LastName = order.User.LastName,
                    Mail = order.User.Mail,
                    Phone = order.User.Phone,
                    Address = new AddressDTO()
                    {
                        Id = order.User.Address.Id,
                        Street = order.User.Address.Street,
                        City = order.User.Address.City,
                        State = order.User.Address.State,
                        Zip = order.User.Address.Zip,
                        Country = order.User.Address.Country,
                    }
                },
            }).ToList(),
            currentPage = response.currentPage,
            maxPage = response.maxPage,
            nextPage = response.nextPage,
            previousPage = response.previousPage
        };

        return View(model);
    }

    // GET: OrdersController/Details/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var order = await this.orderService.GetOrderAsync(id);

            if (order is null)
            {
                return this.NotFound();
            }

            OrderDTO orderDto = new OrderDTO()
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                Status = order.Status,
                User = new UserDTO()
                {
                    Id = order.User.Id,
                    FirstName = order.User.FirstName,
                    LastName = order.User.LastName,
                    Mail = order.User.Mail,
                    Phone = order.User.Phone,
                    Address = new AddressDTO()
                    {
                        Id = order.User.Address.Id,
                        Street = order.User.Address.Street,
                        City = order.User.Address.City,
                        State = order.User.Address.State,
                        Zip = order.User.Address.Zip,
                        Country = order.User.Address.Country,
                    }
                },
            };

            var products = await this.orderService.GetOrderProductsAsync(id);

            return this.View(new OrderDetailsViewModel()
            {
                Order = orderDto,
                Products = products
            });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La commande n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> Create()
    {
        var users = await this.orderService.GetOrderClientsAsync();
        var orderCreateViewModel = new OrderCreateViewModel()
        {
            UserList = users.Select(user => new SelectListItem($"{user.FirstName.ToUpper()} {user.LastName}", user.Id.ToString())).ToList()
        };
        return this.View(orderCreateViewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Create([Bind("OrderDate,DeliveryDate,UserId,Status")] OrderCreateViewModel orderCreateViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(orderCreateViewModel);

        try
        {
            var newOrder = new OrderCreateOrUpdateDTO
            {
                OrderDate = orderCreateViewModel.OrderDate,
                DeliveryDate = orderCreateViewModel.DeliveryDate,
                Status = orderCreateViewModel.Status,
                UserId = orderCreateViewModel.UserId,
            };

            await this.orderService.CreateOrderAsync(newOrder);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la création de la commande";
            return RedirectToAction(nameof(Index));
        }
    }


    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Edit(int id)
    {
        try
        {
            var order = await this.orderService.GetOrderAsync(id);

            if (order is null)
            {
                return this.NotFound();
            }

            var users = await this.orderService.GetOrderClientsAsync();

            var orderEditViewModel = new OrderEditViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                Status = order.Status,
                UserId = order.User.Id,
                UserList = users.Select(user => new SelectListItem($"{user.FirstName.ToUpper()} {user.LastName}", user.Id.ToString())).ToList()
            };

            return this.View(orderEditViewModel);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La commande n'existe pas ou a été supprimé!";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: OrdersController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Edit(int id, [Bind("Id,OrderDate,DeliveryDate,UserId,Status")] OrderEditViewModel orderEditViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(orderEditViewModel);

        try
        {
            var editOrder = new OrderCreateOrUpdateDTO()
            {
                OrderDate = orderEditViewModel.OrderDate,
                DeliveryDate = orderEditViewModel.DeliveryDate,
                Status = orderEditViewModel.Status,
                UserId = orderEditViewModel.UserId,
            };

            await this.orderService.UpdateOrderAsync(id, editOrder);

            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la modification de la commande";
            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var order = await this.orderService.GetOrderAsync(id);

            if (order is null)
            {
                return this.NotFound();
            }

            OrderDTO orderDto = new OrderDTO()
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                Status = order.Status,
                User = new UserDTO()
                {
                    Id = order.User.Id,
                    FirstName = order.User.FirstName,
                    LastName = order.User.LastName,
                    Mail = order.User.Mail,
                    Phone = order.User.Phone,
                    Address = new AddressDTO()
                    {
                        Id = order.User.Address.Id,
                        Street = order.User.Address.Street,
                        City = order.User.Address.City,
                        State = order.User.Address.State,
                        Zip = order.User.Address.Zip,
                        Country = order.User.Address.Country,
                    }
                },
            };

            var products = await this.orderService.GetOrderProductsAsync(id);

            return this.View(new OrderDetailsViewModel()
            {
                Order = orderDto,
                Products = products
            });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "La commande n'existe pas ou a été supprimé!";
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
            await this.orderService.DeleteOrderAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Echec de la suppréssion de la commande";
            return RedirectToAction(nameof(Index));
        }
    }


    [HttpGet("Orders/{id}/Products/Add")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddProduct(int id)
    {
        var products = await this.productService.GetProductsAsync();

        this.ViewData["orderSelected"] = id;
        this.ViewData["orderSelectedId"] = id;

        return this.View(new OrderProductAddViewModel()
        {
            ProductList = products.Select(product => new SelectListItem(product.Name, product.Id.ToString())).ToList(), 
            ProductId = 0
        });
    }

    // POST: OrdersController/Edit/5
    [HttpPost("Orders/{id}/Products/Add")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AddProduct(int id, [Bind("ProductId")] OrderProductAddViewModel orderProductAddViewModel)
    {
        if (!this.ModelState.IsValid)
            return this.View(orderProductAddViewModel);

        try
        {
            var product = await this.productService.GetProductAsync(orderProductAddViewModel.ProductId);
            await this.orderService.UpdateOrderAddProductAsync(id, product);

            return this.RedirectToAction(nameof(this.Details), new { id = id });
        }
        catch
        {
            return this.View();
        }
    }

    [HttpGet("Orders/{orderId}/Products/{id}/Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteProduct(int orderId, int id)
    {
        var order = await this.orderService.GetOrderAsync(orderId);

        this.ViewData["orderSelected"] = orderId;
        this.ViewData["orderSelectedId"] = orderId;

        var product = await this.productService.GetProductAsync(id);

        var productEditViewModel = new OrderProductDeleteViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            OrderId = orderId
        };

        return this.View(productEditViewModel);
    }

    // POST: DrinksController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteProductConfirmed(int orderId, int id)
    {
        try
        {
            await this.orderService.UpdateOrderDeleteProductAsync(orderId, id);
            return this.RedirectToAction(nameof(this.Details), new { id = orderId });
        }
        catch
        {
            return this.RedirectToAction(nameof(this.Details), new { id = orderId });
        }
    }
}