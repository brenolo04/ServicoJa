using ServicoJa.Domain.Enums;
using ServicoJa.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ServicoJa.Application.UseCases.OrdemServico.Criar;

public record CriarOrdemServicoRequest
{
    [Required]
    public long IdServico { get; init; }
    
    public long IdPerfilSolicitante { get; set; }
    
    public string? NomeSolicitante { get; set; }

    [Required]
    [Length(8, 8)]
    public string Cep { get; set; }
    
    public string? Numero { get; set; }
    
    [Required]
    public DateTime DataMarcado { get; set; }
}
