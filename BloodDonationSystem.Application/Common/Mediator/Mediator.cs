using Microsoft.Extensions.DependencyInjection;

namespace BloodDonationSystem.Application.Common.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);
        
        var handler = _serviceProvider.GetRequiredService(handlerType);

        
        var method = handler.GetType().GetMethod("Handle", new[] { requestType, typeof(CancellationToken) });
        
        if (method == null)
        {
            throw new InvalidOperationException($"Método 'Handle' não encontrado no manipulador {handler.GetType().Name} para a requisição {requestType.Name}");
        }

        var task = (Task)method.Invoke(handler, new object[] { request, cancellationToken });
        await task;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        
        var handler = _serviceProvider.GetRequiredService(handlerType);
        
        var method = handler.GetType().GetMethod("Handle", new[] { requestType, typeof(CancellationToken) });

        if (method == null)
        {
            throw new InvalidOperationException($"Método 'Handle' não encontrado no manipulador {handler.GetType().Name} para a requisição {requestType.Name} com resposta {typeof(TResponse).Name}");
        }

        var resultTask = (Task<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken });
        return await resultTask;
    }
}
