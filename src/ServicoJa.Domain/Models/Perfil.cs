namespace ServicoJa.Domain.Models;

public class Perfil : EntidadeBase
{
    public Perfil(long idUsuarioIdentity, string nome)
    {
        IdUsuarioIdentity = idUsuarioIdentity;
        Nome = nome;
    }

    public long IdUsuarioIdentity { get; init; }
    public string Nome { get; init; }
}
