using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductsManagementService _service;

    public ProductsController(IProductsManagementService service)
    {
        _service = service; 
    }

    [HttpGet()]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _service.GetAllProducts();
        if (products == null || !products.Any()) return NoContent();
        return Ok(products);
    }


    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        try 
        {
            var product = await _service.GetProductById(id);
            return Ok(product);
        }
        catch (EntityNotFoundException nt)
        {
            return NotFound(nt.Message);
        }
        
    }

    [HttpPost()]
    public async Task<IActionResult> AddProduct([FromBody]ProductModel.RequestProductModel request)
    {
        try
        {
            var product = await _service.AddProduct(request);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product); 
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch(ApplicationException de)
        {
            return BadRequest(de.Message);
        }
        catch (Exception)
        {
            return Problem("An error occurred while saving the product");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductModel.RequestProductModel request)
    {
        try
        {
            var updatedProduct = await _service.UpdateProduct(id, request);
            return Ok(updatedProduct);
        }
        catch (EntityNotFoundException ex) 
        {
            return NotFound(ex.Message); 
        }
        catch (BadRequestException ae)
        {
            return BadRequest(ae.Message); 
        }
        catch (Exception)
        {
            return Problem("An error occurred while saving the product"); 
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchProduct(Guid id)
    {
        try
        {
            await _service.PatchProduct(id);
            return NoContent();
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (ApplicationException de)
        {
            return NotFound(de.Message);
        }
        catch (Exception)
        {
            return Problem("An error occurred while saving the product");
        }

    }
}
