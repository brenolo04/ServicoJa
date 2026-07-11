using FluentResults;

namespace ServicoJa.Domain.Results;

public class DomainError : Error
{
    public DomainError(string Motivo, long Id)
        : base($"Motivo: {Motivo}. Identificador da entidade: {Id}")
    {
        
    }
}
