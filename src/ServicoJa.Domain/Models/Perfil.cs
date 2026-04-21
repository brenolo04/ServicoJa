namespace ServicoJa.Domain.Models;

public class Perfil : EntidadeBase
{
    public Perfil(long idUsuarioIdentity)
    {
        IdUsuarioIdentity = idUsuarioIdentity;
    }

    public long IdUsuarioIdentity { get; init; }

    #region NavigationProperties

    public IList<Servico> Servicos { get; private set; }

    #endregion

    #region Regras

    public IEnumerable<Servico> AdicionarServico(Servico servico)
    {
        Servicos.Add(servico);
        return Servicos;
    }

    #endregion

}
