namespace BloodDonationSystem.Application.Common.Mediator;

public interface IRequest { }

public interface IRequest<out TResponse> : IRequest { }

public interface INotification { }