using ServicoJa.Domain.Models;

namespace ServicoJa.Domain.Interfaces.Repositories;

public interface IPerfilRepository
{
    Task<Perfil?> ObterPerfilPorIdAsync(long idPerfil);
}
