public record UserNameChanged(string Name, int UserId) : AuditedEvent(UserId);
