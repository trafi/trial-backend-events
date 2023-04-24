using System.Text.Json;
using AutoFixture;
using Confluent.Kafka;
using EventContracts;

namespace EventProducer;

public class UserService
{
    private const string TopicName = "internal_user_updated";

    private readonly Repository _repository;
    private readonly Random _random;
    private readonly ProducerConfig _producerConfig;

    private readonly Randomizer _randomizer;
    private readonly IFixture _fixture = new Fixture();

    public UserService(
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

        using var producer = new ProducerBuilder<int, string>(_producerConfig)
            .Build();

        while (true)
        {
            var message = new Message<int, string>
            {
                Key = eventKey++,
                Value = JsonSerializer.Serialize(new EventEnvelope<UserUpdatedEvent>(GetNextEvent(), eventKey))
            };

            await producer.ProduceAsync(TopicName, message);
            Console.WriteLine(TopicName + " new event sent");

            await Task.Delay(TimeSpan.FromSeconds(_random.Next(1, 5)));
        }
    }

    private UserUpdatedEvent GetNextEvent()
    {
        UserUpdatedEvent @event;
        var isNewUser = _random.Next(0, 10) > 8;
        if (isNewUser)
        {
            @event = GenerateNew();
            _repository.TrackUser(@event);
            return @event;
        }

        @event = _repository.GetRandomUser() ?? GenerateNew();
        @event = ModifyExisting(@event);
        
        _repository.TrackUser(@event);

        return @event;
    }

    private UserUpdatedEvent GenerateNew() =>
        _fixture.Build<UserUpdatedEvent>()
            .With(x => x.Email, _randomizer.GenerateName(_random.Next(10, 15)) + "@trafi.com")
            .With(x => x.LastName, _random.Next(0, 10) > 5 ? _randomizer.GenerateName(_random.Next(10, 15)) : null)
            .With(x => x.FirstName, _random.Next(0, 10) > 5 ? _randomizer.GenerateName(_random.Next(10, 15)) : null)
            .With(x => x.Id, Guid.NewGuid().ToString)
            .Create();
    
    private UserUpdatedEvent ModifyExisting(UserUpdatedEvent @event) =>
        _fixture.Build<UserUpdatedEvent>()
            .With(x => x.Email, @event.Email)
            .With(x => x.Id, @event.Id)
            .With(x => x.LastName, _random.Next(0, 10) > 5 ? _randomizer.GenerateName(_random.Next(10, 15)) : @event.LastName)
            .With(x => x.FirstName, _random.Next(0, 10) > 5 ? _randomizer.GenerateName(_random.Next(10, 15)) :  @event.FirstName)
            .Create();
}