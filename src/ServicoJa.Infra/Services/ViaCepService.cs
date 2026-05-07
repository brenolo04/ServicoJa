using ServicoJa.Domain.Interfaces.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServicoJa.Infra.Services;

public class ViaCepService : IEnderecoService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ViaCepService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<EnderecoExternoDto?> EnderecoPorCep(string cep)
    {
        var client = _httpClientFactory.CreateClient("ViaCep");

        var response = await client.GetAsync($"ws/{cep}/json");

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStreamAsync();
        var content = await JsonSerializer.DeserializeAsync<ViaCepResponse>(json);

        return new EnderecoExternoDto(content.Logradouro, content.Bairro, content.Localidade);
    }
}

public record ViaCepResponse
{
    [JsonPropertyName("logradouro")]
    public string Logradouro { get; init; }
    [JsonPropertyName("bairro")]
    public string Bairro { get; init; }
    [JsonPropertyName("localidade")]
    public string Localidade { get; init; }
}