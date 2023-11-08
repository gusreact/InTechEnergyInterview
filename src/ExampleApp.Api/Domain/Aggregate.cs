namespace ExampleApp.Api.Domain;

internal class Aggregate<T> where T : notnull
{
    public T Id { get; protected init; } = default!;
    public DateTimeOffset CreatedOn { get; init; }
    public DateTimeOffset LastModifiedOn { get; protected set; }
}
