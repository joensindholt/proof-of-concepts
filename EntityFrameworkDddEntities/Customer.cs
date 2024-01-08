namespace EntityFrameworkDddEntities;

public class Customer
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Customer(string name)
    {
        Name = name;
    }
}