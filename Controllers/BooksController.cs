using Microsoft.AspNetCore.Mvc;
using ActiveMQ.Artemis.Client;
using active_mq_app.Model;
using active_mq_app.Amq;

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
        await _context.Books.AddAsync(newBook);
        await _context.SaveChangesAsync();
        
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
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        book.Title = command.Title;
        book.Author = command.Author;
        book.Cost = command.Cost;
        book.InventoryAmount = command.InventoryAmount;

        _context.Books.Update(book);
        await _context.SaveChangesAsync();

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