using Application.Users.Commands;

using ApplicationTests.Common;

using Domain.Persistence.Contexts;

namespace ApplicationTests.Users.Commands;

internal class ChangeRoleTests : IDisposable
{
    private readonly AuthenticationDbContext _context;
    private ChangeRole.Handler _handler;
    public ChangeRoleTests()
    {
        _context = ContextFactory.Create();
        _handler = new ChangeRole.Handler(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }


}
