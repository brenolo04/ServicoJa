namespace ServicoJa.Application.UseCases.Servico.ObterTodos;

public record ObterTodosServicosResponse(IEnumerable<ServicosSaida> Servicos, int TotalRegistros);

public record ServicosSaida(long Id, string Nome, string Descricao);