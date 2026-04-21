namespace ServicoJa.Domain.ValueObjects;

public record Endereco
{
    public string Rua;
    public string Bairro;
    public string Cidade;
    public string? Numero;
    public string Cep;

    public Endereco(string rua, string bairro, string cidade, string cep, string numero = null)
    {
        Rua = rua;
        Bairro = bairro;
        Cidade = cidade;
        Numero = numero;
        Cep = cep;
    }
}
