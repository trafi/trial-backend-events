namespace EventContracts;

public interface IEvent
{
}

public record EventEnvelope<T>(T Event, int Id) where T : IEvent;

public enum BookingType
{
    Unknow = 0,
    Scooter = 1,
    Rental = 2,
    RideHailing = 3
}

public record BookingUpdatedEvent(
    string Id, string UserId,
    string? From, string? To,
    BookingType Type, 
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate,
    double Price
) : IEvent;

public record UserUpdatedEvent(
        string Id, 
        string? LastName, 
        string? FirstName, 
        string? Email) : IEvent;
