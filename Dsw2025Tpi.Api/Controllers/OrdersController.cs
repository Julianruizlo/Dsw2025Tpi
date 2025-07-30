using Azure.Core;
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

    public async Task<IActionResult> AddOrder([FromBody] OrderModel.RequestOrderModel request)
    {
        try
        {
            var orders = await _service.AddOrder(request);
            return CreatedAtAction(nameof(GetOrderById), new { id = orders.Id }, orders);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (InvalidOperationException ioe)
        {
            return BadRequest(ioe.Message);
        }
        catch (ApplicationException de)
        {
            return Conflict(de.Message);
        }
        catch (Exception)
        {
            return Problem("An error occurred while saving the order.");
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        try
        {
            var order = await _service.GetOrderById(id);
            return Ok(order);
        }
        catch (InvalidOperationException ioe)
        {
            return NotFound(ioe.Message);
        }
        
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] OrderModel.RequestOrderModel request)
    {
        try
        {
            var updatedOrder = await _service.PutOrder(id, request);
            if (updatedOrder == null) return NotFound();
            return Ok(updatedOrder);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
        catch (ApplicationException de)
        {
            return Conflict(de.Message);
        }
        catch (Exception)
        {
            return Problem("An error occurred while changing the order status.");
        }
    }
}

