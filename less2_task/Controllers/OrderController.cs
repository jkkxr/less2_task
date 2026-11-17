using AutoMapper;
using less2_task.Models;
using less2_task.Models.DTO;
using less2_task.Services;
using Microsoft.AspNetCore.Mvc;

namespace less2_task.Controllers;

[ApiController]
[Route("api/orders")]
[Tags("Order Management")]
public class OrderController(OrderService orderService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all orders
    /// </summary>
    /// <returns>A collection of all orders with their statuses</returns>
    /// <response code="200">Returns the list of all orders</response>
    [HttpGet]
    [EndpointName("GetAllOrders")]
    [EndpointDescription("A list of all existing orders")]
    [EndpointSummary("Get all orders")]
    [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await orderService.GetAllOrdersAsync();
        var response = mapper.Map<List<OrderResponse>>(orders);
        return Ok(response);
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    /// <param name="orderRequest">The request containing quantity, price, and comment</param>
    /// <returns>The created order with id, date, and text status</returns>
    /// <response code="200">Returns the successfully created order</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPost]
    [EndpointName("AddOrder")]
    [EndpointDescription("Create a new order")]
    [EndpointSummary("Add a new order")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddOrder([FromBody] CreateOrderRequest orderRequest)
    {
        var order = mapper.Map<Order>(orderRequest);
        var orderAdd = await orderService.AddOrderAsync(order);
        var response = mapper.Map<OrderResponse>(orderAdd);
        return Ok(response);
    }

    /// <summary>
    /// Update order quantity
    /// </summary>
    /// <param name="id">The unique id of the order</param>
    /// <param name="orderRequest">The request containing the new quantity</param>
    /// <returns>The updated order object</returns>
    /// <response code="200">Returns the updated order</response>
    /// <response code="404">If the order with specified id is not found</response>
    [HttpPatch("{id:int}/quantity")]
    [EndpointName("UpdateQuantity")]
    [EndpointDescription("Update the item quantity for an existing order by its id")]
    [EndpointSummary("Update order quantity")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateQuantity([FromRoute] int id, [FromBody] UpdateOrderQuantityRequest orderRequest)
    {
        var order = await orderService.UpdateOrderQuantityAsync(id, orderRequest.Quantity);
        if(order == null)
        {
            return NotFound();
        }
        var response = mapper.Map<OrderResponse>(order);
        return Ok(response);
    }

    /// <summary>
    /// Change order status
    /// </summary>
    /// <param name="id">The unique id of the order</param>
    /// <param name="orderRequest">The request containing the new completion status</param>
    /// <returns>The updated order object with the new status</returns>
    /// <response code="200">Returns the updated order</response>
    /// <response code="404">If the order with specified id is not found</response>
    [HttpPatch("{id:int}/status")]
    [EndpointName("ChangeStatus")]
    [EndpointDescription("Change the completion status (completed/not completed) of an order")]
    [EndpointSummary("Change order status")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] ChangeOrderStatusRequest orderRequest)
    {
        var order = await orderService.UpdateOrderStatusAsync(id, orderRequest.IsCompleted);
        if (order == null)
        {
            return NotFound();
        }
        var response = mapper.Map<OrderResponse>(order);
        return Ok(response); ;
    }
}
