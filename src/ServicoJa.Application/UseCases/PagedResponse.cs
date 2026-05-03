namespace ServicoJa.Application.UseCases;

public record PagedResponse
{
    public IEnumerable<object>? Items { get; init; }
    public int PaginaAtual { get; init; }
    public int TamanhoPagina { get; init; }
    public int TotalPaginas => (int)Math.Ceiling(TotalRegistros / (double)TamanhoPagina);
    public int TotalRegistros { get; init; }
    public bool ProximaPagina => PaginaAtual < TotalPaginas;
}
