using System.Collections.Concurrent;
using EventEase.Models;

namespace EventEase.Services;

public sealed record AttendanceSummary(
    int EventId,
    string EventName,
    DateTime Date,
    string Location,
    int RegisteredCount,
    int AttendedCount);

public sealed class EventAttendanceService
{
    private readonly ConcurrentDictionary<int, ConcurrentDictionary<string, AttendeeRecord>> registrations = new();

    public bool TryRegister(EventItem eventItem, string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        var attendees = registrations.GetOrAdd(
            eventItem.Id,
            _ => new ConcurrentDictionary<string, AttendeeRecord>(StringComparer.OrdinalIgnoreCase));
        var normalizedEmail = email.Trim();
        var record = new AttendeeRecord(eventItem.Name, name.Trim(), normalizedEmail, DateTimeOffset.UtcNow, null);

        return attendees.TryAdd(normalizedEmail, record);
    }

    public bool IsRegistered(int eventId, string email) =>
        registrations.TryGetValue(eventId, out var attendees) && attendees.ContainsKey(email.Trim());

    public bool IsAttended(int eventId, string email) =>
        registrations.TryGetValue(eventId, out var attendees) &&
        attendees.TryGetValue(email.Trim(), out var record) &&
        record.CheckedInAt is not null;

    public bool CheckIn(int eventId, string email)
    {
        if (!registrations.TryGetValue(eventId, out var attendees))
        {
            return false;
        }

        var normalizedEmail = email.Trim();
        while (attendees.TryGetValue(normalizedEmail, out var current))
        {
            if (current.CheckedInAt is not null)
            {
                return true;
            }

            var updated = current with { CheckedInAt = DateTimeOffset.UtcNow };
            if (attendees.TryUpdate(normalizedEmail, updated, current))
            {
                return true;
            }
        }

        return false;
    }

    public AttendanceSummary GetSummary(EventItem eventItem)
    {
        registrations.TryGetValue(eventItem.Id, out var attendees);
        var records = attendees?.Values;

        return new AttendanceSummary(
            eventItem.Id,
            eventItem.Name,
            eventItem.Date,
            eventItem.Location,
            attendees?.Count ?? 0,
            records?.Count(record => record.CheckedInAt is not null) ?? 0);
    }

    public AttendanceSummary[] GetSummaries(IEnumerable<EventItem> eventItems) =>
        eventItems.Select(GetSummary).ToArray();

    private sealed record AttendeeRecord(
        string EventName,
        string Name,
        string Email,
        DateTimeOffset RegisteredAt,
        DateTimeOffset? CheckedInAt);
}