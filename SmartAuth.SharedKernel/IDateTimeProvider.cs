namespace SmartAuth.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
