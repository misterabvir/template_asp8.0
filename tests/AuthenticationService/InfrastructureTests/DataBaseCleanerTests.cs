using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using Infrastructure;
using Infrastructure.Persistence.Contexts;
using InfrastructureTests.Constants;
using NSubstitute;
using Quartz;
using Shared.Domain;

namespace InfrastructureTests;

public class DataBaseCleanerTests
{
    private readonly AuthenticationDbContext _context;
    private readonly DataBaseCleaner.Settings _settings;
    private readonly DataBaseCleaner.CleanerJob _job;
    private readonly List<User> _users;
    private readonly List<IDomainEvent> _events;
    private readonly IJobExecutionContext _jobExecution;

    public DataBaseCleanerTests()
    {
        _context = ContextFactory.GetContext();
        _settings = new DataBaseCleaner.Settings()
        {
            IntervalInSeconds = 10,
            NotVerifiedTimeLimitInHours = 0,
            OldMessagesTimeLimitInDays = 0,
            JobKey = "DataBaseCleaner"
        };
        _job = new DataBaseCleaner.CleanerJob(_context, _settings);
        _jobExecution = Substitute.For<IJobExecutionContext>();
        _jobExecution.CancellationToken.Returns(CancellationToken.None);
        _users = ContextFactory.GetNotVerifiedUsers(_context, Random.Shared.Next(1, 10));
        _events = ContextFactory.GetProcessedDomainEvents(_context, Random.Shared.Next(1, 10));
    }

    [Fact]
    public async Task Execute_ShouldBeClearAllNotVerifiedUsersAndProcessedDomainEvents()
    {
        //Arrange

        // Act
        await _job.Execute(_jobExecution);

        // Asserts
        var users = _context.Users.Where(x=>x.Status == Status.NotVerified).ToList();
        users.Should().BeEmpty();
        var messages = _context.OutboxMessages.Where(x=> x.ProcessedAt != null).ToList();
        messages.Should().BeEmpty();
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
