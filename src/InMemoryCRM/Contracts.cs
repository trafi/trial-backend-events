namespace InMemoryCRM;

public record CRMUser(
    string Id,
    string LastName,
    string? FirstName,
    string Email,
    string? LastTripType,
    string? LastTripId,
    DateTimeOffset LastTripDate);

public record CRMTrip(
    string Id,
    string UserId,
    string? From,
    string? To,
    double? Price,
    DateTimeOffset TripStartDate,
    DateTimeOffset? TripEndDate,
    string TripType);
    