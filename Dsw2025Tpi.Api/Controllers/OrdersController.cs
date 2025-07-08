using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersManagementService _service;

    public OrdersController(IOrdersManagementService service)
    {
        _service = service;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _service.GetAllOrders();
        if (orders == null || !orders.Any()) return NoContent();
        return Ok(orders);
    }

    [HttpPost]

     public async Task<IActionResult> AddOrder([FromBody]OrderModel.RequestOrderModel request)
    {
        try
        {
            var orders = await _service.AddOrder(request);
            return Ok(orders); 
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch(ApplicationException de)
        {
            return Conflict(de.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al guardar la orden");
        }
    }

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetOrderById(Guid id)
    {
        return Task.FromResult<IActionResult>(Ok());
    }



}

