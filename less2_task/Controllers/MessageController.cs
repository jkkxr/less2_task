using less2_task.Models;
using System.ComponentModel;
using less2_task.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace less2_task.Controllers;

/// <summary>
/// Controller for managing messages
/// </summary>
[Tags("Message Management")]
[ApiController]
[Route("api/messages")]
public class MessageController(MessageService messageService, IMapper mapper) : ControllerBase
{
    private bool CheckUser()
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Get all messages
    /// </summary>
    /// <returns>A list of all messages</returns>
    /// <response code="200">Returns the list of all messages</response>
    /// <response code="401">If the user is unauthorized</response>
    [ProducesResponseType(typeof(List<Message>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<IActionResult> GetAllMessages()
    {
        if (!CheckUser())
        {
            return Unauthorized();
        }

        var messages = await messageService.GetAllMessagesAsync();
        var messagesViewModel = mapper.Map<List<Message>>(messages);
        return Ok(messagesViewModel);
    }

    /// <summary>
    /// Get a specific message by its id
    /// </summary>
    /// <param name="id">The unique id of the message</param>
    /// <returns>The requested message object</returns>
    /// <response code="200">Returns the requested message</response>
    /// <response code="401">If the user is unauthorized</response>
    [ProducesResponseType(typeof(Message), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMessageById(int id)
    {
        throw new Exception("Test exception");
        if (!CheckUser())
        {
            return Unauthorized();
        }
        var message = await messageService.GetMessageByIdAsync(id);
        if (message == null)
        {
            return NotFound(); 
        }
        var messagesViewModel = mapper.Map<Message>(message);
        return Ok(messagesViewModel);
    }

    /// <summary>
    /// Add a new message
    /// </summary>
    /// <param name="newMessage">The message object to add</param>
    /// <returns>Adds a new message object</returns>
    /// <response code = "200">Returns the successfully new message</response>
    /// <response code="401">If the user is unauthorized</response>
    [ProducesResponseType(typeof(Message), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    public async Task<IActionResult> AddMessage([FromBody] Message newMessage)
    {
        if (!CheckUser())
        {
            return Unauthorized();
        }
        var messageAdd = mapper.Map<Message>(newMessage);
        var message = await messageService.AddMessageAsync(messageAdd);
        return Ok(message);
    }

    /// <summary>
    /// Delete message by its id
    /// </summary>
    /// <param name="id">The unique id of the message to delete</param>
    /// <returns>Nothing if it's successful</returns>
    /// <response code = "200">If the message was successfully deleted</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        if (!CheckUser())
        {
            return Unauthorized();
        }
        var result = await messageService.DeleteMessageAsync(id);
        if (!result)
        {
            return NotFound(); 
        }
        return Ok(); 
    }

    /// <summary>
    /// Get messages by a certain sender's email
    /// </summary>
    /// <param name="sender">The sender's email</param>
    /// <returns>A list of messages sent by the specified sender</returns>
    /// <response code ="200">Returns the list of messages from this sender</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpGet("sender/{sender}")]
    public async Task<IActionResult> GetMessageBySender(string sender)
    {
        if (!CheckUser())
        {
            return Unauthorized();
        }
        var messages = await messageService.GetMessagesBySender(sender);
        var messagesViewModel = mapper.Map<List<Message>>(messages);
        return Ok(messagesViewModel);
    }
}
