namespace EventProducer;

public class Randomizer
{
    private readonly Random _random;

    public Randomizer(Random random)
    {
        _random = random;
    }
    
    public string GenerateName(int len)
    { 
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        string Name = "";
        Name += consonants[_random.Next(consonants.Length)].ToUpper();
        Name += vowels[_random.Next(vowels.Length)];
        int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len)
        {
            Name += consonants[_random.Next(consonants.Length)];
            b++;
            Name += vowels[_random.Next(vowels.Length)];
            b++;
        }

        return Name;
    }

    public DateTimeOffset GetRandomDate(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        TimeSpan timeSpan = endDate - startDate;
        TimeSpan newSpan = new TimeSpan(0, _random.Next(0, (int)timeSpan.TotalMinutes), 0);
        return startDate + newSpan;
    }
}