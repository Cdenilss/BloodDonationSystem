namespace BloodDonationSystem.Application.Services.ViaCep;

public interface IViaCepService
{
    Task<ViaCepAddressResponse?> GetAddressByCepAsync(string cep);
}