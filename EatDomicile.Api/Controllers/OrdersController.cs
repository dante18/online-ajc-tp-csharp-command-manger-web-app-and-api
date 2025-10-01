using EatDomicile.Api.Dtos.Address;
using EatDomicile.Api.Dtos.Ingredient;
using EatDomicile.Api.Dtos.Order;
using EatDomicile.Api.Dtos.User;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService orderService;

        private readonly UserService userService;

        public OrdersController(OrderService orderService, UserService userService)
        {
            this.orderService = orderService;
            this.userService = userService;
        }

        [HttpGet]
        public IResult GetOrders()
        {
            List<OrderDto> orders = this.orderService.GetAllOrders().Select(o => new OrderDto()
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

            return Results.Ok(orders);
        }

        [HttpGet("{id}")]
        public IResult GetOrder([FromRoute] int id)
        {
            Order order = this.orderService.GetOrder(id);
            if (order is null)
                return Results.NotFound($"Order not found by id : {id}");

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

        [HttpGet("{id}/products")]
        public IResult GetOrderProducts([FromRoute] int id)
        {
            Order order = this.orderService.GetOrder(id);
            if (order is null)
                return Results.NotFound($"Order not found by id : {id}");

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

        [HttpPost()]
        public IResult CreateOrder([FromBody] CreateOrUpdateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            User user = this.userService.GetUser(dto.UserId);

            Order order = new Order()
            {
                OrderDate = dto.OrderDate,
                DeliveryDate = dto.DeliveryDate,
                Status = dto.Status,
                UserId = dto.UserId,
                DeliveryAddressId = user.Address.Id
            };

            this.orderService.CreateOrder(order);

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

        [HttpPut("{id}")]
        public IResult UpdateOrder([FromRoute] int id, [FromBody] CreateOrUpdateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            Order order = this.orderService.GetOrder(id);
            if (order is null)
                return Results.NotFound($"Order not found by id : {id}");

            if (dto.OrderDate != null) order.OrderDate = dto.OrderDate;
            if (dto.DeliveryDate != null) order.DeliveryDate = dto.DeliveryDate;
            if (dto.Status != null) order.Status = dto.Status;
            if (dto.UserId != null) order.UserId = dto.UserId;

            this.orderService.UpdateOrder(order);

            return Results.NoContent();
        }

        [HttpPost("{id}/products")]
        public IResult UpdateOrderAddProduct([FromRoute] int id, [FromBody] CreateOrderProductDto dto)
        {
            Order order = this.orderService.GetOrder(id);
            if (order is null)
                return Results.NotFound($"Order not found by id : {id}");

            List<int> productIds = order.OrderProduct.Select(op => op.ProductId).ToList();
            productIds.Add(dto.Id);

            this.orderService.UpdateOrderAddProduct(order, productIds);

            return Results.NoContent();
        }

        [HttpDelete("{id}/products/{productId}")]
        public IResult UpdateOrderDeleteProduct([FromRoute] int id, [FromRoute] int productId)
        {
            Order order = this.orderService.GetOrder(id);
            if (order is null)
                return Results.NotFound($"Order not found by id : {id}");

            this.orderService.UpdateOrderRemoveProduct(order, productId);

            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteOrder(int id)
        {
            Order order = this.orderService.GetOrder(id);
            if (order is null)
                return Results.NotFound($"Order not found by id : {id}");

            this.orderService.DeleteOrder(order);

            return Results.NoContent();
        }
    }
}
