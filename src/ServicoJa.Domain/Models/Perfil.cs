namespace ServicoJa.Domain.Models;

public class Perfil : EntidadeBase
{
    public Perfil(long idUsuarioIdentity)
    {
        IdUsuarioIdentity = idUsuarioIdentity;
    }

    public long IdUsuarioIdentity { get; init; }
    public IList<OrdemServico> OrdemServicos { get; private set; }
    public IList<Servico> Servicos { get; private set; }

    #region NavigationProperties

    #endregion

    #region Regras

    public void AtualizarPefil()
    {
    }

    public IEnumerable<OrdemServico> AdicionarOrdemServico(OrdemServico ordemServico)
    {
        OrdemServicos.Add(ordemServico);
        return OrdemServicos;
    }

    public IEnumerable<Servico> AdicionarServico(Servico servico)
    {
        Servicos.Add(servico);
        return Servicos;
    }

    #endregion

}
