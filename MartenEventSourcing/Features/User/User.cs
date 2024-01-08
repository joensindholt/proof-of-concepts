namespace MartenEventSourcing.Features.User;

public class User
{
  public Guid Id { get; set; }

  public string? Name { get; set; }

  public void Apply(UserCreated created)
  {
    Id = Guid.NewGuid();
    Name = created.Name;
  }

  public void Apply(UserNameChanged changed)
  {
    Name = changed.Name;
  }

  public override string ToString()
  {
    return $"{Id}: {Name}";
  }
}