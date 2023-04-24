using System.Collections.Concurrent;
using EventContracts;

namespace EventProducer;

public class Repository
{
    private readonly Random _random;
    
    private readonly ConcurrentDictionary<string, UserUpdatedEvent> _userUpdatedEvents = new();
    private readonly ConcurrentDictionary<string, BookingUpdatedEvent> _bookingUpdatedEvents = new();

    public Repository(Random random)
    {
        _random = random;
    }

    public void TrackUser(UserUpdatedEvent @event)
    {
        _userUpdatedEvents.TryAdd(@event.Id, @event);
    }

    public UserUpdatedEvent? GetRandomUser()
    {
        if (!_userUpdatedEvents.Any())
            return default;
        
        return _userUpdatedEvents.Values
            .ElementAtOrDefault(_random.Next(0, _userUpdatedEvents.Values.Count - 1));
    }
    
    public void TrackBooking(BookingUpdatedEvent @event)
    {
        _bookingUpdatedEvents.TryAdd(@event.Id, @event);
    }

    public BookingUpdatedEvent? GetRandomBooking()
    {
        if (!_bookingUpdatedEvents.Any())
            return default;
        
        return _bookingUpdatedEvents.Values
            .ElementAtOrDefault(_random.Next(0, _userUpdatedEvents.Values.Count - 1));
    }
}