using Domain.UserAggregate;
using Domain.UserAggregate.Events;
using Domain.UserAggregate.ValueObjects;

using Infrastructure;
using Infrastructure.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Shared.Domain;

namespace InfrastructureTests.Constants;

public static class ContextFactory
{
    public record TestDomainEvent() : IDomainEvent;
    public record TestDomainEventWithNumber(int Number) : IDomainEvent;
    public record TestDomainEventWithString(string Text) : IDomainEvent;

    public static AuthenticationDbContext GetContext()
    {
        var builder = new DbContextOptionsBuilder<AuthenticationDbContext>()
            .UseInMemoryDatabase(databaseName: "XXXXXXXXXXXXXXXXXXXXXXX");
        var context = new AuthenticationDbContext(builder.Options);
        context.Database.EnsureCreated();
        return context;
    }



    public static List<IDomainEvent> GetDomainEvents(AuthenticationDbContext context, int count = 3)
    {
        List<IDomainEvent> events = [];
        for (int i = 0; i < count; i++)
        {
            var random = Random.Shared.Next(3);
            switch(random) 
            {
                case 0:
                    {
                        var @event = new TestDomainEvent();
                        if (RegisteredDomainEvents.Events.ContainsKey(nameof(TestDomainEvent)) == false)
                        {
                            RegisteredDomainEvents.Events.Add(nameof(TestDomainEvent), typeof(TestDomainEvent));
                        }
                        break;
                    }
                case 1:
                    {
                        var @event = new TestDomainEventWithNumber(Random.Shared.Next(1000));
                        if (RegisteredDomainEvents.Events.ContainsKey(nameof(TestDomainEventWithNumber)) == false)
                        {
                            RegisteredDomainEvents.Events.Add(nameof(TestDomainEventWithNumber), typeof(TestDomainEventWithNumber));
                        }
                        break;
                    }
                case 2:
                    {
                        var @event = new TestDomainEventWithString(Guid.NewGuid().ToString());
                        if(RegisteredDomainEvents.Events.ContainsKey(nameof(TestDomainEventWithString)) == false)
                        {
                            RegisteredDomainEvents.Events.Add(nameof(TestDomainEventWithString), typeof(TestDomainEventWithString));
                        }

                        break;
                    }
                default:
                    break;
            };
            
        }

        context.OutboxMessages.AddRange(events.Select(e => new Outbox.Message()
        {
            Content = JsonConvert.SerializeObject(e),
            Type = e.GetType().Name,
            CreatedAt = DateTime.UtcNow.AddDays(-30)
        }));
        context.SaveChanges();
        return events;
    }

    public static List<IDomainEvent> GetProcessedDomainEvents(AuthenticationDbContext context, int count = 3)
    {
        var events = GetDomainEvents(context, count);
        context.OutboxMessages.ToList().ForEach(e => e.ProcessedAt = DateTime.UtcNow);
        context.SaveChanges();
        return events;
    }
    public static List<User> GetNotVerifiedUsers(AuthenticationDbContext context, int count = 3)
    {
        List<User> users = [];
        for (int i = 0; i < count; i++)
        {
            var user = User.Create(
                Username.Create(Guid.NewGuid().ToString()), 
                Email.Create(Guid.NewGuid().ToString()), 
                Password.Create(Guid.NewGuid().ToByteArray()), 
                Salt.Create(Guid.NewGuid().ToByteArray()));
            users.Add(user);
        }
        context.Users.AddRange(users);
        context.SaveChanges();
        return users;
    }
}

