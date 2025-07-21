using FluentValidation;

namespace BloodDonationSystem.Application.Common.Mediator
{
    /// <summary>
    /// Este é o nosso "posto de controle de qualidade".
    /// Ele intercepta todos os comandos (IRequest) antes de chegarem aos seus Handlers.
    /// </summary>
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // 1. Verifica se existe algum validador registrado para este comando específico.
            if (!_validators.Any())
            {
                // Se não houver, apenas continua para o próximo passo no pipeline (que pode ser outro behavior ou o handler final).
                return await next();
            }

            // 2. Cria um contexto de validação para o comando atual.
            var context = new ValidationContext<TRequest>(request);

            // 3. Executa todos os validadores encontrados de forma assíncrona.
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // 4. Agrupa todos os erros de validação de todos os validadores em uma única lista.
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // 5. Se a lista de erros não estiver vazia, lança uma única exceção com todos os erros.
            if (failures.Count != 0)
            {
                // Isso interrompe a execução do pipeline. O Handler do comando NUNCA será chamado.
                throw new ValidationException(failures);
            }

            // 6. Se não houver erros, a validação passou. Continua a execução para o Handler.
            return await next();
        }
    }
}