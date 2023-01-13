using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using active_mq_app.Model;
using active_mq_app.Amq;
using active_mq_app.Commands;
using System.Threading.Tasks;
using active_mq_app.Contracts;

namespace active_mq_app.Controllers;

 [ApiController]
 [Route("[controller]")]
 public class BooksController : ControllerBase
 {
    private readonly MessageProducer _messageProducer;

    public BooksController(MessageProducer messageProducer)
    {
        _messageProducer = messageProducer;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateBook command)
    {
        var newBook = new Book
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Author = command.Author,
            Cost = command.Cost,
            InventoryAmount = command.InventoryAmount,
        };
        
        var @event = new BookCreated
        {
            Id = newBook.Id,
            Title = newBook.Title,
            Author = newBook.Author,
            Cost = newBook.Cost,
            InventoryAmount = newBook.InventoryAmount,
            UserId = command.UserId,
            Timestamp = DateTime.UtcNow
        };
        await _messageProducer.PublishAsync(@event);
        
        return StatusCode((int) HttpStatusCode.Created, new { newBook.Id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateBook command)
    {
        var book = new Book();

        book.Title = command.Title;
        book.Author = command.Author;
        book.Cost = command.Cost;
        book.InventoryAmount = command.InventoryAmount;

     
        var @event = new BookUpdated
        {
            Id = book.Id,
            Author = book.Author,
            Cost = book.Cost,
            Title = book.Title,
            InventoryAmount = book.InventoryAmount,
            UserId = command.UserId,
            Timestamp = DateTime.UtcNow
        };
        
        await _messageProducer.PublishAsync(@event);

        return Ok();
    }
}