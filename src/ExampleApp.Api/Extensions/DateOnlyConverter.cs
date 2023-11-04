using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ExampleApp.Api.Extensions;

public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
        dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
        dateTime => DateOnly.FromDateTime(dateTime))
    { }
}
