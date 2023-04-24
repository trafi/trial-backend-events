using System.Text.Json;
using AutoFixture;
using Confluent.Kafka;
using EventContracts;

namespace EventProducer;

public class BookingService
{
    private const string TopicName = "internal_booking_updated";

    private readonly Repository _repository;
    private readonly Randomizer _randomizer;
    private readonly Random _random;
    private readonly ProducerConfig _producerConfig;

    private IFixture _fixture = new Fixture();

    public BookingService(
        Repository repository,
        Random random,
        Randomizer randomizer,
        ProducerConfig producerConfig)
    {
        _repository = repository;
        _random = random;
        _randomizer = randomizer;
        _producerConfig = producerConfig;
    }
    
    public async Task Run()
    {
        int eventKey = 0;

        using var producer = new ProducerBuilder<int, string>(_producerConfig).Build();

        while (true)
        {
            var message = new Message<int, string>
            {
                Key = eventKey++,
                Value = JsonSerializer.Serialize(new EventEnvelope<BookingUpdatedEvent>(GetNextEvent(), eventKey))
            };

            await producer.ProduceAsync(TopicName, message);
            Console.WriteLine(TopicName + " new event sent");

            await Task.Delay(TimeSpan.FromSeconds(_random!.Next(1, 5)));
        }
    }
    
    private BookingUpdatedEvent GetNextEvent()
    {
        var userId =  _repository.GetRandomUser()?.Id;
        
        BookingUpdatedEvent @event;
        var isNewUser = _random.Next(0, 10) > 2;
        if (isNewUser)
        {
            @event = GenerateNew(userId);
            _repository.TrackBooking(@event);
            return @event;
        }

        @event = _repository.GetRandomBooking() ?? GenerateNew(userId);
        @event = ModifyExisting(@event);
        
        _repository.TrackBooking(@event);

        return @event;
    }

    private BookingUpdatedEvent GenerateNew(string? userId) =>
        _fixture.Build<BookingUpdatedEvent>()
            .With(x => x.Id, Guid.NewGuid().ToString())
            .With(x => x.From, _random.Next(0, 10) > 5 ? _randomizer.GenerateName(_random.Next(10, 15)) : null)
            .With(x => x.To, _random.Next(0, 10) > 5 ? _randomizer.GenerateName(_random.Next(10, 15)) : null)
            .With(x => x.Price, _random.Next(0, 10) > 5 ? _random.NextDouble() : 0D)
            .With(x => x.UserId, userId)
            .With(x => x.Type, (BookingType) _random.Next(0, 3))
            .With(x => x.StartDate, DateTimeOffset.Now)
            .With(x => x.EndDate, _random.Next(0, 10) > 5 ? _randomizer.GetRandomDate(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(31)) : null)
            .Create();
    
    private BookingUpdatedEvent ModifyExisting(BookingUpdatedEvent @event) =>
        _fixture.Build<BookingUpdatedEvent>()
            .With(x => x.Id,  @event.Id)
            .With(x => x.Type,  @event.Type)
            .With(x => x.StartDate,  @event.StartDate)
            .With(x => x.UserId,  @event.UserId)
            .With(x => x.From, @event.From)
            .With(x => x.To, _random.Next(0, 10) > 5 ? _randomizer.GenerateName(_random.Next(10, 15)) : null)
            .With(x => x.Price, _random.Next(0, 10) > 5 ? _random.NextDouble() : 0D)
            .With(x => x.EndDate, _random.Next(0, 10) > 5 ? _randomizer.GetRandomDate(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(31)) : null)
            .Create();
}