using EatDomicile.Api.Dtos.Address;
using EatDomicile.Api.Dtos.Common;
using EatDomicile.Api.Dtos.Order;
using EatDomicile.Api.Dtos.User;
using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> logger;

    private readonly IOrderService orderService;

    private readonly IUserService userService;

    public OrdersController(ILogger<OrdersController> logger, IOrderService orderService, IUserService userService)
    {
        this.logger = logger;
        this.orderService = orderService;
        this.userService = userService;
        this.logger.LogInformation("OrdersController initialized");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> GetOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        this.logger.LogInformation("GetOrders called");

        try
        {
            var queryResult = this.orderService.GetAllOrders();
            var queryResultWithPagination = await queryResult.ToPagedResultAsync(pageNumber, pageSize);

            List<OrderDto> orders = queryResultWithPagination.Items.Select(o => new OrderDto()
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                DeliveryDate = o.DeliveryDate,
                Status = o.Status,
                User = new UserDto()
                {
                    Id = o.User.Id,
                    FirstName = o.User.FirstName,
                    LastName = o.User.LastName,
                    Mail = o.User.Mail,
                    Phone = o.User.Phone,
                    Address = new AddressDto()
                    {
                        Id = o.User.Address.Id,
                        Street = o.User.Address.Street,
                        City = o.User.Address.City,
                        State = o.User.Address.State,
                        Zip = o.User.Address.Zip,
                        Country = o.User.Address.Country,
                    }
                }
            }).ToList();

            PaginatedResponse<OrderDto> response = new PaginatedResponse<OrderDto>()
            {
                Items = orders,
                currentPage = pageNumber,
                maxPage = queryResultWithPagination.TotalPages,
                nextPage = pageNumber == queryResultWithPagination.TotalPages
                    ? queryResultWithPagination.TotalPages
                    : (pageNumber + 1),
                previousPage = pageNumber == 1 ? 1 : (pageNumber - 1)
            };

            this.logger.LogInformation("GetOrders completed successfully. Found {OrderCount} orders", orders.Count);

            return Results.Ok(response);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching orders");

            return Results.InternalServerError();
        }
    }

    [HttpGet("order-clients")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> GetOrderClients()
    {
        this.logger.LogInformation("GetOrders called");

        try
        {
            List<UserDto> clients = this.userService.GetAllUsers().Select(u => new UserDto()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Phone = u.Phone,
                Mail = u.Mail,
                Address = new AddressDto()
                {
                    Id = u.Address.Id,
                    Street = u.Address.Street,
                    City = u.Address.City,
                    State = u.Address.State,
                    Zip = u.Address.Zip,
                    Country = u.Address.Country
                }
            }).ToList();

            this.logger.LogInformation("GetOrderClients completed successfully. Found {OrderClientCount} order clients", clients.Count);

            return Results.Ok(clients);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching order clients");

            return Results.InternalServerError();
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public IResult GetOrder([FromRoute] int id)
    {
        this.logger.LogInformation("GetOrder called with ID: {OrderId}", id);

        try
        {
            Order order = this.orderService.GetOrder(id);

            if (order is null)
            {
                this.logger.LogWarning("Order not found with ID: {OrderId}", id);
                return Results.NotFound($"Order not found by id : {id}");
            }

            this.logger.LogInformation("GetOrder completed successfully for ID: {OrderId}, OrderDate: {OrderDate}",
                id,
                order.OrderDate);

            OrderDto orderDto = new OrderDto()
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                Status = order.Status,
                User = new UserDto()
                {
                    Id = order.User.Id,
                    FirstName = order.User.FirstName,
                    LastName = order.User.LastName,
                    Mail = order.User.Mail,
                    Phone = order.User.Phone,
                    Address = new AddressDto()
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

            return Results.Ok(orderDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching order with ID: {OrderId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpGet("{id}/products")]
    [Authorize(Roles = "Admin")]
    public IResult GetOrderProducts([FromRoute] int id)
    {
        this.logger.LogInformation("GetOrderProducts called with ID: {OrderId}", id);

        try
        {
            Order order = this.orderService.GetOrder(id);

            if (order is null)
            {
                this.logger.LogWarning("Order not found with ID: {OrderId}", id);
                return Results.NotFound($"Order not found by id : {id}");
            }

            this.logger.LogInformation("GetOrderProducts completed successfully for ID: {OrderId}, OrderDate: {OrderName}",
                id,
                order.OrderDate);

            List<OrderProductDto> products = new List<OrderProductDto>();
            if (order.OrderProduct is not null)
            {
                products = order.OrderProduct.Select(op => new OrderProductDto()
                {
                    Id = op.Id,
                    Product = new ProductDto()
                    {
                        Id = op.Product.Id ?? 0,
                        Name = op.Product.Name,
                        Price = op.Product.Price
                    }
                }).ToList();
            }

            return Results.Ok(products);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching products order with ID: {OrderId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPost()]
    [Authorize(Roles = "Admin")]
    public IResult CreateOrder([FromBody] CreateOrUpdateOrderDto dto)
    {
        User user = this.userService.GetUser(dto.UserId);

        this.logger.LogInformation("CreateOrder called with OrderDate: {OrderDate}, DeliveryDate: {DeliveryDate}, Status: {Status}, User: {UserId}, Address: {AddressId}",
            dto.OrderDate,
            dto.DeliveryDate,
            dto.Status,
            dto.UserId,
            user.Address.Id
            );

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Order order = new Order()
            {
                OrderDate = dto.OrderDate,
                DeliveryDate = dto.DeliveryDate,
                Status = dto.Status,
                UserId = dto.UserId,
                DeliveryAddressId = user.Address.Id
            };

            this.logger.LogDebug("Adding new order to context: {OrderDetails}",
                new
                {
                    dto.OrderDate,
                    dto.DeliveryDate,
                    dto.Status,
                    dto.UserId,
                    user.Address.Id
                }
            );

            this.orderService.CreateOrder(order);

            this.logger.LogInformation("CreateOrder completed successfully. New order ID: {OrderId}, Name: {Name}",
                order.Id,
                order.OrderDate);

            OrderDto orderDto = new OrderDto()
            {
                OrderDate = order.OrderDate,
                DeliveryDate = dto.DeliveryDate,
                Status = dto.Status,
                User = new UserDto()
                {
                    Id = order.User.Id,
                    FirstName = order.User.FirstName,
                    LastName = order.User.LastName,
                    Mail = order.User.Mail,
                    Phone = order.User.Phone,
                    Address = new AddressDto()
                    {
                        Id = order.User.Address.Id,
                        Street = order.User.Address.Street,
                        City = order.User.Address.City,
                        State = order.User.Address.State,
                        Zip = order.User.Address.Zip,
                        Country = order.User.Address.Country,
                    }
                }
            };

            return Results.Created($"/api/orders/{order.Id}", orderDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e,
                "An error occurred while creating order with Name: {OrderDate}",
                dto.OrderDate);

            return Results.InternalServerError(e);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdateOrder([FromRoute] int id, [FromBody] CreateOrUpdateOrderDto dto)
    {
        this.logger.LogInformation("UpdateOrder called for ID: {OrderId} with data: {UpdateData}",
            id,
            new
            {
                dto.OrderDate,
                dto.DeliveryDate,
                dto.Status,
                dto.UserId
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Order order = this.orderService.GetOrder(id);

            if (order is null)
            {
                this.logger.LogWarning("Cannot update order - not found with ID: {OrderId}", id);
                return Results.NotFound($"Order not found by id : {id}");
            }

            this.logger.LogDebug("Found existing order: {OrderDate}",
                order.OrderDate);

            if (dto.OrderDate != null) order.OrderDate = dto.OrderDate;
            if (dto.DeliveryDate != null) order.DeliveryDate = dto.DeliveryDate;
            if (dto.Status != null) order.Status = dto.Status;
            if (dto.UserId != null) order.UserId = dto.UserId;

            this.orderService.UpdateOrder(order);

            this.logger.LogInformation("UpdateOrder completed successfully for ID: {OrderId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while updating order with ID: {OrderId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPost("{id}/products")]
    [Authorize(Roles = "Admin")]
    public IResult UpdateOrderAddProduct([FromRoute] int id, [FromBody] CreateOrderProductDto dto)
    {
        this.logger.LogInformation("UpdateOrderAddProduct called for ID: {OrderId} with data: {UpdateData}",
            id,
            new
            {
                dto.Name,
                dto.Price,
                OrderId = id
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Order order = this.orderService.GetOrder(id);

            if (order is null)
            {
                this.logger.LogWarning("Cannot add product - order not found with ID: {OrderId}", id);
                return Results.NotFound($"Order not found by id : {id}");
            }

            this.logger.LogDebug("Found existing order: {OrderDate}",
                order.OrderDate);

            List<int> productIds = order.OrderProduct.Select(op => op.ProductId).ToList();
            productIds.Add(dto.Id);

            this.orderService.UpdateOrderAddProduct(order, productIds);

            this.logger.LogInformation("UpdateOrderAddProduct completed successfully for ID: {OrderId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while adding product to order with ID: {OrderId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpDelete("{id}/products/{productId}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdateOrderDeleteProduct([FromRoute] int id, [FromRoute] int productId)
    {
        this.logger.LogInformation("UpdateOrderDeleteProduct called for ID: {OrderId}", id);

        try
        {
            Order order = this.orderService.GetOrder(id);

            if (order is null)
            {
                this.logger.LogWarning("Cannot delete product - order not found with ID: {OrderId}", id);
                return Results.NotFound($"Order not found by id : {id}");
            }

            this.logger.LogDebug("Found existing order: {OrderDate}",
                order.OrderDate);

            this.orderService.UpdateOrderRemoveProduct(order, productId);

            this.logger.LogInformation("UpdateOrderDeleteProduct completed successfully for ID: {OrderId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while deleting product to order with ID: {OrderId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> DeleteOrder(int id)
    {
        this.logger.LogInformation("DeleteOrder called for ID: {OrderId}", id);

        try
        {
            Order order = this.orderService.GetOrder(id);

            if (order is null)
            {
                this.logger.LogWarning("Cannot delete order - not found with ID: {OrderId}", id);
                return Results.NotFound($"Order not found by id : {id}");
            }

            this.logger.LogInformation("Deleting order: ID={OrderId}, Name='{OrderDate}'",
                order.Id,
                order.OrderDate);

            this.orderService.DeleteOrder(order);

            this.logger.LogInformation("DeleteOrder completed successfully for ID: {OrderId}", id);
            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while deleting order with ID: {OrderId}", id);
            return Results.InternalServerError(e);
        }
    }
}
