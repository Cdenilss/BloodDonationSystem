using System.Threading;
using System.Threading.Tasks;

namespace BloodDonationSystem.Application.Common.Mediator;

public interface IMediator
{
    Task<TResponse> SendWithResponse<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;
    Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;
}