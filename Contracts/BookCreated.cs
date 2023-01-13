using System;

namespace active_mq_app.Contracts;

public record BookCreated()
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Cost { get; set; }
    public int InventoryAmount { get; set; }
    public Guid UserId { get; set; }
    public DateTime Timestamp { get; set; }
}