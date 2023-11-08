namespace ExampleApp.Api.Domain;

internal class ValueObject<T> where T: notnull
{
    public T Id { get; protected init; } = default(T)!;
}
