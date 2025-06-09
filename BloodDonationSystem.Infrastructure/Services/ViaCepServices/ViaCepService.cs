using System.Net.Http.Json;
using BloodDonationSystem.Application.Services.ViaCep;

namespace BloodDonationSystem.Infrastructure.Services.ViaCepServices;

public class ViaCepService: IViaCepService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public async Task<ViaCepAddressResponse?> GetAddressByCepAsync(string cep)
    {
        var cleanCep= new string(cep.Where(char.IsDigit).ToArray());
        
        var httpClient = _httpClientFactory.CreateClient("ViaCep");

        try
        {
            var response= await httpClient.GetFromJsonAsync<ViaCepAddressResponse>($"{cleanCep}/json/");
            
            if(response != null && !response.Erro)
            {
                return null;
            }
            return response;
          

        }
        catch (HttpRequestException e)
        {
            return null;
        }
    }
    
    
}