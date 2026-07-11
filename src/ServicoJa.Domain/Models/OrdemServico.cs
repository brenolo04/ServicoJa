using FluentResults;
using ServicoJa.Domain.Enums;
using ServicoJa.Domain.ValueObjects;

namespace ServicoJa.Domain.Models;

public class OrdemServico : EntidadeBase
{

    #region Construtor

    public OrdemServico(long idPerfilPrestador, string nomeSolicitante, long idServico, DateTime dataMarcado)
    {
        IdPerfilPrestador = idPerfilPrestador;
        IdPerfilSolicitante = null;
        NomeSolicitante = nomeSolicitante;
        SolicitanteAnonimo = true;
        IdServico = idServico;
        DataMarcado = dataMarcado;
    }
    public OrdemServico(long idPerfilPrestador, long idPerfilSolicitante, long idServico, DateTime dataMarcado)
    {
        IdPerfilPrestador = idPerfilPrestador;
        IdPerfilSolicitante = idPerfilSolicitante;
        NomeSolicitante = null;
        SolicitanteAnonimo = false;
        IdServico = idServico;
        DataMarcado = dataMarcado;
    }

    #endregion

    #region Propiedades

    public long IdServico { get; init; }
    public long IdPerfilPrestador { get; init; }
    public long? IdPerfilSolicitante { get; private set; }
    public string? NomeSolicitante { get; private set; }
    public bool SolicitanteAnonimo { get; private set; }
    public Endereco Endereco { get; private set; }
    public DateTime DataMarcado { get; private set; }
    public DateTime? DataFinalizado { get; private set; }
    public DateTime DataCriacao { get; } = DateTime.UtcNow;
    public EStatusServico Status { get; private set; } = EStatusServico.AguardandoAprovacao;

    #endregion

    #region NavigationProperties

    public Perfil PerfilPrestador { get; init; }
    public Perfil? PerfilSolicitante { get; init; }
    public Servico Servico { get; init; }

    #endregion

    #region Regras

    public Result AtualizarSolicitanteAnonimo(string nomeSolicitante)
    {
        if (string.IsNullOrEmpty(nomeSolicitante))
            return Result.Fail("Nome do solicitante anônimo não pode ser vazio.");

        NomeSolicitante = nomeSolicitante;

        return Result.Ok();
    }
    
    public Result VincularEndereco(Endereco endereco)
    {
        var ehAlgumaPropriedadeVazia = 
            string.IsNullOrEmpty(endereco.Cep) || 
            string.IsNullOrEmpty(endereco.Cidade) || 
            string.IsNullOrEmpty(endereco.Bairro) || 
            string.IsNullOrEmpty(endereco.Rua);

        if (ehAlgumaPropriedadeVazia)
            return Result.Fail("Nenhuma informação de endereço pode ser vazia.");

        Endereco = endereco;

        return Result.Ok();
    }
    
    public Result AprovarOrdemServico()
    {
        if (Status != EStatusServico.AguardandoAprovacao)
            return Result.Fail("Só é possível aprovar ordem se Status for igual Aguardando aprovação!");

        Status = EStatusServico.Aprovado;

        return Result.Ok();
    }

    public Result ExecutarOrdemServico()
    {
        if (Status == EStatusServico.Cancelado || Status == EStatusServico.Finalizado)
            return Result.Fail("Só pode ser status Executando quando status for diferente de Cancelado e Finalizado");

        Status = EStatusServico.Executando;

        return Result.Ok();
    }

    public Result FinalizarOrdemServico()
    {
        if (Status == EStatusServico.Cancelado)
            return Result.Fail("Só pode ser status Finalizado quando status for diferente de Cancelado");

        Status = EStatusServico.Finalizado;
        DataFinalizado = DateTime.UtcNow;

        return Result.Ok();
    }

    public void CancelarOrdemServico()
        => Status = EStatusServico.Cancelado;

    #endregion

}
