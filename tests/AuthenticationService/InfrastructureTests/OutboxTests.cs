using Domain.UserAggregate.Events;

using FluentAssertions;

using Infrastructure;
using Infrastructure.BackgroundJobs;
using Infrastructure.Persistence.Contexts;

using InfrastructureTests.Constants;

using MediatR;

using NSubstitute;

using Quartz;

using Shared.Domain;

namespace InfrastructureTests;

public class OutboxTests
{
    private readonly AuthenticationDbContext _context;
    private readonly IPublisher _publisher;
    private readonly Outbox.Settings _settings;
    private readonly Outbox.PublishJob _job;
    private readonly IJobExecutionContext _jobExecution;
    private readonly List<IDomainEvent> _events;


    public OutboxTests()
    {
        _context = ContextFactory.GetContext();
        _events = ContextFactory.GetDomainEvents(_context, Random.Shared.Next(1, 10));
        _publisher = Substitute.For<IPublisher>();
        _settings = new Outbox.Settings()
        {
            MessagePerOneTime = int.MaxValue,
            IntervalInSeconds = 10,
            JobKey = "OutboxJob"
        };

        _jobExecution = Substitute.For<IJobExecutionContext>();
        _jobExecution.CancellationToken.Returns(CancellationToken.None);
        _job = new Outbox.PublishJob(_context, _publisher, _settings);
       
    }

    [Fact]
    public async Task Execute_ShouldPublishMessages_WhenMessagesExist()
    {
        // Arrange

        // Act
        await _job.Execute(_jobExecution);

        // Assert
        await _publisher.Received(_events.Count).Publish(Arg.Any<INotification>(), Arg.Any<CancellationToken>());
        var messages = _context.OutboxMessages.ToList();
        foreach (var message in messages)
        {
            message.ProcessedAt.HasValue.Should().BeTrue();
        }
        _context.Database.EnsureDeleted();
        _context.Dispose();           
    }
}
