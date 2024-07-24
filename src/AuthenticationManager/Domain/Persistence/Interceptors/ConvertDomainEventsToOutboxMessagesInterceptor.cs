using Newtonsoft.Json;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Domain;

namespace Domain.Persistence.Interceptors;


public class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

       
        var messages = new List<Outbox.Message>();
        context.ChangeTracker
            .Entries<IContainDomainEvents>().ToList()
            .ForEach(a=>
            {
                foreach (var d in a.Entity.DomainEvents)
                {
                    messages.Add(new Outbox.Message
                    {
                        Type = d.GetType().Name,
                        Content = JsonConvert.SerializeObject(d)
                    });
                } 
            });



        await context.Set<Outbox.Message>().AddRangeAsync(messages, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}