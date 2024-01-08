using Marten;
using Marten.Events.Projections;
using Microsoft.Extensions.Hosting;
using Weasel.Core;

const string connectionString = "User ID=postgres;Password=mypassword;Host=localhost;Port=5432;Database=myDataBase;";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMarten(options =>
{
    options.Connection(connectionString);

    // If we're running in development mode, let Marten just take care
    // of all necessary schema building and patching behind the scenes
    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }
});

builder.Build().Run();




var store = DocumentStore.For(_ =>
{
    _.Connection(connectionString);
    _.Projections.Snapshot<User>(SnapshotLifecycle.Inline);
    _.Events.MetadataConfig.HeadersEnabled = true;
});

// Insert first user
await using (var session = store.LightweightSession())
{
    session.SetHeader("userId", 123);

    var created = new UserCreated("Jim", 123);

    // Start a brand new stream and commit the new events as
    // part of a transaction
    session.Events.StartStream<User>(created);

    // Save the pending changes to db
    await session.SaveChangesAsync();
}

await using (var session = store.LightweightSession())
{
    session.SetHeader("userId", 123);

    var users = await session.Query<User>().ToListAsync();

    foreach (var u in users)
    {
        Console.WriteLine(u.ToString());
    }

    var user = users[0];
    var changed = new UserNameChanged("Janine", 123);
    session.Events.Append(user.Id, changed);
    await session.SaveChangesAsync();
}
