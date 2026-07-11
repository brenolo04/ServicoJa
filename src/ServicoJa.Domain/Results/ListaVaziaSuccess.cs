using FluentResults;

namespace ServicoJa.Domain.Errors;

public class ListaVaziaSuccess : Success
{  
    public ListaVaziaSuccess(string Entidade)
        : base($"Nenhum registro encontrado de {Entidade}")
    {
    }
}
