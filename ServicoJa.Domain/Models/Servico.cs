namespace ServicoJa.Domain.Models;

public class Servico : EntidadeBase
{
    #region Construtor

    public Servico(long idPerfil, string nome, string descricao, float valor)
    {
        IdPerfil = idPerfil;
        Nome = nome;
        Descricao = descricao;
        Valor = valor;
    }

    #endregion

    #region Propiedades

    public long IdPerfil { get; init; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public float Valor { get; private set; }
    public bool Inativo { get; private set; } = false;
    public DateTime DataCriado { get; } = DateTime.Now;

    #endregion

    #region NavigationProperties
    public Perfil Perfil { get; init; }
    #endregion

    #region Regras

    public void AtualizarServico(string nome, string descricao, float valor)
    {
        Nome = nome;
        Descricao = descricao;
        Valor = valor;
    }

    public void Inativar() => Inativo = true;

    public void Ativar() => Inativo = false;

    #endregion

}
