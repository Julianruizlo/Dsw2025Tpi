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


    [HttpGet("{id}")] //Revisar si conviene poner como lo pone facundo
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
            return Ok(product);
            // return CreatedAtAction(nameof(GetProductByIdAsync), new { id = result.Id }, result); -- Comprobar si no hay problema con ID, si hay usar esto
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
            return Problem("Se produjo un error al guardar el producto");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductModel.RequestProductModel request)
    {
        try
        {
            var updatedProduct = await _service.UpdateProduct(id, request);
            return Ok(updatedProduct); // ya no hace falta chequear si es null
        }
        catch (EntityNotFoundException ex) // o 
        {
            return NotFound(ex.Message); // producto no encontrado
        }
        catch (BadRequestException ae)
        {
            return BadRequest(ae.Message); // errores de validación
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al actualizar el producto"); // error 500 genérico
        }
    }

    [HttpPatch("{id}/TOGGLE")]
    public async Task<IActionResult> PatchProduct(Guid id)
    {
        try
        {
            var patchedProduct = await _service.PatchProduct(id);
            if (patchedProduct == null) return NotFound();
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
            return Problem("Se produjo un error al actualizar el producto");
        }

    }
}
