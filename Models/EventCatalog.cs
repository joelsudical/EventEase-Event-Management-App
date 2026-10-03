using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public sealed class EventItem
{
    public int Id { get; init; }

    [Required(ErrorMessage = "Enter an event name.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Use 3 to 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [FutureDate]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Enter a location.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Use 2 to 120 characters.")]
    public string Location { get; set; } = string.Empty;
    public string Description { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string ImageUrl { get; init; } = string.Empty;
}

public sealed class FutureDateAttribute : ValidationAttribute
{
    public FutureDateAttribute() : base("Choose today or a future date.")
    {
    }

    public override bool IsValid(object? value) =>
        value is DateTime date && date.Date >= DateTime.Today;
}

public static class EventCatalog
{
    public const int MaxEvents = 1_000;

    private static readonly EventItem[] FeaturedEvents =
    [
        new EventItem
        {
            Id = 1,
            Name = "Product Leadership Summit",
            Date = new DateTime(2026, 11, 12),
            Location = "Seattle, WA",
                Description = "A day of practical conversations on building thoughtful products and strong teams.",
                Category = "Conference",
                ImageUrl = "https://images.unsplash.com/photo-1540575467063-178a50c2df87?auto=format&fit=crop&w=480&q=80"
        },
        new EventItem
        {
            Id = 2,
            Name = "Community Innovation Night",
            Date = new DateTime(2026, 12, 4),
            Location = "Portland, OR",
                Description = "Meet local makers and explore new ideas through talks, demos, and conversation.",
                Category = "Community",
                ImageUrl = "https://images.unsplash.com/photo-1511632765486-a01980e01a18?auto=format&fit=crop&w=480&q=80"
        },
        new EventItem
        {
            Id = 3,
            Name = "Year-End Partner Reception",
            Date = new DateTime(2026, 12, 17),
            Location = "Vancouver, BC",
                Description = "An evening reception celebrating the people and partnerships behind a great year.",
                Category = "Reception",
                ImageUrl = "https://images.unsplash.com/photo-1511795409834-ef04bbd61622?auto=format&fit=crop&w=480&q=80"
        }
    ];

    public static List<EventItem> GetEvents(int count = 3)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, MaxEvents);

        return Enumerable.Range(1, count).Select(CreateEvent).ToList();
    }

    public static EventItem? GetEvent(int id) =>
        id is >= 1 and <= MaxEvents ? CreateEvent(id) : null;

    private static EventItem CreateEvent(int id)
    {
        var featuredEvent = FeaturedEvents[(id - 1) % FeaturedEvents.Length];
        var seriesNumber = (id - 1) / FeaturedEvents.Length;

        return new EventItem
        {
            Id = id,
            Name = seriesNumber == 0 ? featuredEvent.Name : $"{featuredEvent.Name} #{seriesNumber + 1}",
            Date = featuredEvent.Date.AddDays(seriesNumber * 7),
            Location = featuredEvent.Location,
              Description = featuredEvent.Description,
              Category = featuredEvent.Category,
              ImageUrl = featuredEvent.ImageUrl
        };
    }
}