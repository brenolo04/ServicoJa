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

    public void AtualizarSolicitanteAnonimo(string nomeSolicitante)
    {
        if (string.IsNullOrEmpty(nomeSolicitante))
            throw new AppDomainUnloadedException("Nome do solicitante anônimo não pode ser vazio.");

        NomeSolicitante = nomeSolicitante;
    }
    
    public void VincularEndereco(Endereco endereco)
    {
        var ehAlgumaPropriedadeVazia = 
            string.IsNullOrEmpty(endereco.Cep) || 
            string.IsNullOrEmpty(endereco.Cidade) || 
            string.IsNullOrEmpty(endereco.Bairro) || 
            string.IsNullOrEmpty(endereco.Rua);

        if (ehAlgumaPropriedadeVazia)
            throw new AppDomainUnloadedException("Cep, Cidade, Bairro e Rua do endereço não pode ser vazios");

        Endereco = endereco;
    }
    
    public void AprovarOrdemServico()
    {
        if(Status != EStatusServico.AguardandoAprovacao)
            throw new AppDomainUnloadedException("Só pode ser status Aprovado quando status for Aguardando Aprovação");

        Status = EStatusServico.Aprovado;
    }

    public void ExecutarOrdemServico()
    {
        if (Status == EStatusServico.Cancelado || Status == EStatusServico.Finalizado)
            throw new AppDomainUnloadedException("Só pode ser status Executando quando status for diferente de Cancelado e Finalizado");

        Status = EStatusServico.Executando;
    }

    public void FinalizarOrdemServico()
    {
        if (Status == EStatusServico.Cancelado)
            throw new AppDomainUnloadedException("Só pode ser status Finalizado quando status for diferente de Cancelado");

        Status = EStatusServico.Finalizado;
        DataFinalizado = DateTime.UtcNow;
    }

    public void CancelarOrdemServico()
        => Status = EStatusServico.Cancelado;

    #endregion

}
