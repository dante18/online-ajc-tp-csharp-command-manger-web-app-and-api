using EatDomicile.Api.Dtos.Order;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService productService;

    public ProductsController(ProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    public IResult GetProducts()
    {
        List<ProductDto> products = this.productService.GetAllProducts().Select(p => new ProductDto()
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        }).ToList();

        return Results.Ok(products);
    }

    [HttpGet("{id}")]
    public IResult GetProduct([FromRoute] int id)
    {
        Product product = this.productService.GetProduct(id);
        if (product is null)
            return Results.NotFound($"Product not found by id : {id}");

        ProductDto productDto = new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };

        return Results.Ok(productDto);
    }
}
