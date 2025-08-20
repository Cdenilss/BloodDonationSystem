using BloodDonationSystem.Application.Services.ViaCep;
using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Application.Mappings;

public static class AddressMappings
{
    public static Address ToEntity(this ViaCepAddressResponse viaCepResponse, string number, string? complement)
    {
        return new Address(
            street: viaCepResponse.Logradouro ?? string.Empty,
            city: viaCepResponse.Localidade ?? string.Empty,
            state: viaCepResponse.Uf ?? string.Empty,
            zipCode: viaCepResponse.Cep?.Replace("-", "") ?? string.Empty,
            number: number,
            complement: complement,
            district: viaCepResponse.Bairro ?? string.Empty
        );
    }
}