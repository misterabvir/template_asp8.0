using MailService.Application.Common.Repositories;

using MailService.Domain;

using MediatR;

namespace MailService.Application.Queries;

public static class GetLastEmails
{
    public record Query(int Amount) : IRequest<IEnumerable<Message>>;
    public class Handler(IMessageRepository messageRepository) : IRequestHandler<Query, IEnumerable<Message>>
    {
        public async Task<IEnumerable<Message>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await messageRepository.GetLastEmails(request.Amount, cancellationToken);
        }
    }
}

public static class GetLastUserEmails
{
    public record Query(Guid UserId, int Amount) : IRequest<IEnumerable<Message>>;
    public class Handler(IMessageRepository messageRepository) : IRequestHandler<Query, IEnumerable<Message>>
    {
        public async Task<IEnumerable<Message>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await messageRepository.GetLastUserEmails(request.UserId, request.Amount, cancellationToken);
        }
    }
}