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

    public IEnumerable<OrdemServico> AdicionarPrestacaoDeServico(OrdemServico ordemServico)
    {
        OrdemServicosPrestados.Add(ordemServico);
        return OrdemServicosPrestados;
    }
    
    public IEnumerable<OrdemServico> AdicionarSolicitacaoDeServico(OrdemServico ordemServico)
    {
        OrdemServicosSolicitados.Add(ordemServico);
        return OrdemServicosSolicitados;
    }

    public IEnumerable<Servico> AdicionarServico(Servico servico)
    {
        Servicos.Add(servico);
        return Servicos;
    }

    #endregion

}
