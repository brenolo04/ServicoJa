namespace ServicoJa.Domain.Models;

public class Perfil : EntidadeBase
{
    public Perfil(long idUsuarioIdentity)
    {
        IdUsuarioIdentity = idUsuarioIdentity;
    }

    public long IdUsuarioIdentity { get; init; }

    #region NavigationProperties

    public IList<OrdemServico> OrdemServicosPrestados { get; private set; }
    public IList<OrdemServico> OrdemServicosSolicitados { get; private set; }
    public IList<Servico> Servicos { get; private set; }

    #endregion

    #region Regras

    public void AdicionarPrestacaoDeServico(OrdemServico ordemServico)
    {
        if (ordemServico.IdPerfilPrestador != Id) 
            throw new AppDomainUnloadedException();

        OrdemServicosPrestados.Add(ordemServico);
    }
    
    public void AdicionarSolicitacaoDeServico(OrdemServico ordemServico)
    {
        if (ordemServico.IdPerfilSolicitante != Id)
            throw new AppDomainUnloadedException();

        OrdemServicosSolicitados.Add(ordemServico);
    }

    public IEnumerable<Servico> AdicionarServico(Servico servico)
    {
        Servicos.Add(servico);
        return Servicos;
    }

    #endregion

}
