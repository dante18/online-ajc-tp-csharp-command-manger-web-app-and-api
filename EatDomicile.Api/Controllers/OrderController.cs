using EatDomicile.Api.Dtos.Address;
using EatDomicile.Api.Dtos.Order;
using EatDomicile.Api.Dtos.User;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        public OrderController(OrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public IResult GetUOrders()
        {
            List<OrderDto> orders = this.orderService.GetAllOrders().Select(o => new OrderDto()
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                DeliveryDate = o.DeliveryDate,
                Status = o.Status,
                UserId = o.UserId
            }).ToList();

            return Results.Ok(orders);
        }

        [HttpGet("{id}")]
        public IResult GetUser([FromRoute] int id)
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
                UserId = order.UserId
            };

            return Results.Ok(orderDto);
        }

        [HttpPost()]
        public IResult CreateOrder([FromBody] CreateOrUpdateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            Order order = new Order()
            {
                OrderDate = dto.OrderDate,
                DeliveryDate = dto.DeliveryDate,
                Status = dto.Status,
                UserId = dto.UserId
            };

            this.orderService.CreateOrder(order);

            OrderDto orderDto = new OrderDto()
            {
                OrderDate = order.OrderDate,
                DeliveryDate = dto.DeliveryDate,
                Status = dto.Status,
                UserId = dto.UserId
            };

            return Results.Created($"/api/orders/{order.Id}", orderDto);
        }

        [HttpPut("{id}")]
        public IResult UpdateOrder([FromRoute] int id, [FromBody] CreateOrUpdateOrderDto dto)
        {
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
