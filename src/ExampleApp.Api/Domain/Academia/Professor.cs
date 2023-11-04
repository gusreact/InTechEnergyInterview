namespace ExampleApp.Api.Domain.Academia;

internal class Professor : ValueObject<int>
{
    public string FullName { get; init; } = "TBD";
    public string? Extension { get; init; }
}
