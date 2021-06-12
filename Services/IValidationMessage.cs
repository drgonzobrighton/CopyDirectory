namespace Services
{
    public interface IValidationMessage
    {
        string Message { get; init; }
        ValidationMessageType MessageType { get; init; }
    }
}
