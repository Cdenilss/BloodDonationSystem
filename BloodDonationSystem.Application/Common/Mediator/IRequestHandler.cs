using System.Threading;
using System.Threading.Tasks;

namespace BloodDonationSystem.Application.Common.Mediator;

public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
    Task Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface INotificationHandler<in TNotification> where TNotification : INotification
{
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}