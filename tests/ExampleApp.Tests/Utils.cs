using Microsoft.Extensions.Logging;

namespace ExampleApp.Tests;

public class Utils
{
    public static ILogger<T> CreateLogger<T>()
        => Substitute.For < ILogger<T>>();
}
