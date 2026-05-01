using ServicoJa.Domain.ValueObjects;

namespace ServicoJa.Domain.Interfaces.Services;

public interface IEnderecoService
{
    Task<Endereco> EnderecoPorCep(string cep);
}
