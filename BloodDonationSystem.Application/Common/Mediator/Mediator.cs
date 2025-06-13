using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BloodDonationSystem.Application.Common.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly ConcurrentDictionary<Type, MethodInfo> _handlerMethods = new();

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> SendWithResponse<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetRequiredService(handlerType);

        var method = _handlerMethods.GetOrAdd(handlerType, t => t.GetMethod("Handle")!);

        var behaviors = _serviceProvider.GetServices(typeof(IPipelineBehavior<,>).MakeGenericType(request.GetType(), typeof(TResponse)))
                                        .Cast<object>().ToList();

        RequestHandlerDelegate<TResponse> handlerDelegate = () => (Task<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken })!;

        foreach (var behavior in behaviors.Reverse<object>())
        {
            var next = handlerDelegate;
            handlerDelegate = () => ((dynamic)behavior).Handle((dynamic)request, cancellationToken, next);
        }

        return await handlerDelegate();
    }

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(typeof(TRequest));
        var handler = _serviceProvider.GetRequiredService(handlerType);

        var method = _handlerMethods.GetOrAdd(handlerType, t => t.GetMethod("Handle")!);

        return (Task)method.Invoke(handler, new object[] { request, cancellationToken })!;
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();
        foreach (var handler in handlers)
        {
            await handler.Handle(notification, cancellationToken);
        }
    }
}