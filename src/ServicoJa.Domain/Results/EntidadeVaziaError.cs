using FluentResults;

namespace ServicoJa.Domain.Results;

public class EntidadeVaziaError : Error
{
    public EntidadeVaziaError(string Entidade, long Id)
        : base($"Não foi possível encontrar a(o) {Entidade} com Id '{Id}'")
    {
        
    }

}
