namespace EventEase.Services;

public sealed class UserSessionState
{
    private readonly HashSet<int> registeredEventIds = [];
    private readonly HashSet<int> attendedEventIds = [];

    public event Action? Changed;

    public string? Name { get; private set; }
    public string? Email { get; private set; }
    public int RegisteredEventCount => registeredEventIds.Count;

    public bool IsRegistered(int eventId) => registeredEventIds.Contains(eventId);
    public bool HasAttended(int eventId) => attendedEventIds.Contains(eventId);

    public void RecordRegistration(int eventId, string name, string email)
    {
        Name = name.Trim();
        Email = email.Trim();
        registeredEventIds.Add(eventId);
        Changed?.Invoke();
    }

    public void RecordAttendance(int eventId)
    {
        if (attendedEventIds.Add(eventId))
        {
            Changed?.Invoke();
        }
    }

    public void EndSession()
    {
        Name = null;
        Email = null;
        registeredEventIds.Clear();
        attendedEventIds.Clear();
        Changed?.Invoke();
    }
}