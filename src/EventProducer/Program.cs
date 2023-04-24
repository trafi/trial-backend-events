// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using EventProducer;

Console.WriteLine("Hello, World!");

var random = new Random((int) DateTime.Now.Ticks);
var randomizer = new Randomizer(random);
var repository = new Repository(random);

var config = new ProducerConfig
{
    BootstrapServers = "localhost:29092",
    ClientId = "trail-day-producer",
};

var userTopic = new UserService(repository, random, randomizer, config).Run();
var bookingTopic = new BookingService(repository, random, randomizer, config).Run();

await userTopic;
await bookingTopic;