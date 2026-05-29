using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.UseCases.Products.Commands;
using ECommerce.Application.UseCases.Products.Dtos;
using ECommerce.Application.UseCases.Products.Queries;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IGetAllProductsUseCase _getAllProductsUseCase;
    private readonly IGetProductByIdUseCase _getProductByIdUseCase;
    private readonly ICreateProductUseCase _createProductUseCase;
    private readonly IDeleteProductUseCase _deleteProductUseCase;

    public ProductController(
        IGetAllProductsUseCase getAllProductsUseCase,
        IGetProductByIdUseCase getProductByIdUseCase,
        ICreateProductUseCase createProductUseCase,
        IDeleteProductUseCase deleteProductUseCase)
    {
        _getAllProductsUseCase = getAllProductsUseCase;
        _getProductByIdUseCase = getProductByIdUseCase;
        _createProductUseCase = createProductUseCase;
        _deleteProductUseCase = deleteProductUseCase;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _getAllProductsUseCase.ExecuteAsync();

        return Ok(products);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _getProductByIdUseCase.ExecuteAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var product = await _createProductUseCase.ExecuteAsync(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.CategoryId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteProductUseCase.ExecuteAsync(id);

        return NoContent();
    }
}
