using EatDomicile.Api.Dtos.Order;
using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> logger;

    private readonly IProductService productService;

    public ProductsController(ILogger<ProductsController> logger, IProductService productService)
    {
        this.logger = logger;
        this.productService = productService;
        this.logger.LogInformation("ProductsController initialized");
    }

    [HttpGet]
    [AllowAnonymous]
    public IResult GetProducts()
    {
        this.logger.LogInformation("GetProducts called");

        try
        {
            List<ProductDto> products = this.productService.GetAllProducts().Select(p => new ProductDto()
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();

            this.logger.LogInformation("GetProducts completed successfully. Found {ProductCount} products", products.Count);

            return Results.Ok(products);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching products");

            return Results.InternalServerError();
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IResult GetProduct([FromRoute] int id)
    {
        this.logger.LogInformation("GetProduct called with ID: {ProductId}", id);

        try
        {
            Product product = this.productService.GetProduct(id);
            if (product is null)
            {
                this.logger.LogWarning("Product not found with ID: {ProductId}", id);
                return Results.NotFound($"Product not found by id : {id}");
            }

            this.logger.LogInformation("GetProduct completed successfully for ID: {ProductId}, Name: {ProductName}",
                id,
                product.Name);

            ProductDto productDto = new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            return Results.Ok(productDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching product with ID: {ProductId}", id);
            return Results.InternalServerError(e);
        }
    }
}
