namespace ServicoJa.Domain.Interfaces.Services;

public interface IEnderecoService
{
    Task<EnderecoExternoDto?> EnderecoPorCep(string cep);
}

public record EnderecoExternoDto(string Logradouro, string Bairro, string Localidade);