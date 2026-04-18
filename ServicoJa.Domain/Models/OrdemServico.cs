using ServicoJa.Domain.Enums;
using ServicoJa.Domain.ValueObjects;

namespace ServicoJa.Domain.Models;

public class OrdemServico : EntidadeBase
{

    #region Construtor

    public OrdemServico(long idPrestador, string nomeSolicitante, long idServico, DateTime dataMarcado, Endereco endereco)
    {
        IdPrestador = idPrestador;
        IdSolicitante = null;
        NomeSolicitante = nomeSolicitante;
        SolicitanteAnonimo = true;
        IdServico = idServico;
        Endereco = endereco;
        DataMarcado = dataMarcado;
    }
    public OrdemServico(long idPrestador, long idSolicitante, long idServico, DateTime dataMarcado, Endereco endereco)
    {
        IdPrestador = idPrestador;
        IdSolicitante = idSolicitante;
        NomeSolicitante = null;
        SolicitanteAnonimo = false;
        IdServico = idServico;
        Endereco = endereco;
        DataMarcado = dataMarcado;
    }

    #endregion

    #region Propiedades

    public long IdServico { get; init; }
    public long IdPrestador { get; init; }
    public long? IdSolicitante { get; private set; }
    public string? NomeSolicitante { get; private set; }
    public bool SolicitanteAnonimo { get; private set; }
    public Endereco Endereco { get; private set; }
    public DateTime DataMarcado { get; private set; }
    public DateTime DataFinalizado { get; private set; }
    public DateTime DataCriacao { get; } = DateTime.Now;
    public EStatusServico Status { get; private set; } = EStatusServico.AguardandoAprovacao;

    #endregion

    #region NavigationProperties

    public Perfil Prestador { get; init; }
    public Perfil? Solicitante { get; init; }
    public Servico Servico { get; init; }

    #endregion

    #region Regras

    public void AtualizarOrdemServico(long? idSolicitante, string nomeSolicitante, Endereco endereco, DateTime dataMarcado)
    {

        if (!string.IsNullOrEmpty(nomeSolicitante))
        {
            IdSolicitante = null;
            NomeSolicitante = nomeSolicitante;
            SolicitanteAnonimo = true;
        }
        else if (idSolicitante is > 0 && idSolicitante != null)
        {
            IdSolicitante = idSolicitante;
            NomeSolicitante = null;
            SolicitanteAnonimo = false;
        }
        else
            throw new AppDomainUnloadedException();

        Endereco = endereco;
        DataMarcado = dataMarcado;

    }

    public void FinalizarOrdemServico()
    {
        if (Status == EStatusServico.Cancelado)
            throw new AppDomainUnloadedException();

        Status = EStatusServico.Finalizado;
        DataFinalizado = DateTime.Now;
    }

    public void CancelarOrdemServico()
        => Status = EStatusServico.Cancelado;

    #endregion

}
