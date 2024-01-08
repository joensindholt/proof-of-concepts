public record UserCreated(string? Name, int UserId) : AuditedEvent(UserId);
