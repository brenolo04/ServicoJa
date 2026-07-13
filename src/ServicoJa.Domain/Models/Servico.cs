using FluentResults;
using ServicoJa.Domain.Results;

namespace ServicoJa.Domain.Models;

public class Servico : EntidadeBase
{
    #region Construtor

    public Servico(long idPerfil, string nome, string descricao, decimal valor)
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
    public decimal Valor { get; private set; }
    public bool Inativo { get; private set; } = false;
    public DateTime DataCriado { get; } = DateTime.UtcNow;

    #endregion

    #region NavigationProperties
    public Perfil Perfil { get; init; }
    #endregion

    #region Regras

    public Result AtualizarServico(string nome, string descricao, decimal valor)
    {

        if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(descricao))
            return Result.Fail(new DomainError("Nome e descrição devem conter algum conteúdo", Id));

        if (valor <= 0)
            return Result.Fail(new DomainError("Valor deve ser maior que zero", Id));

        Nome = nome;
        Descricao = descricao;
        Valor = valor;

        return Result.Ok();
    }

    public void Inativar() => Inativo = true;

    public void Ativar() => Inativo = false;

    #endregion

}
