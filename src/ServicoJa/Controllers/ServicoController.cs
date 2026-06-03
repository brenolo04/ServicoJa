using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoJa.Application.UseCases;
using ServicoJa.Application.UseCases.Servico.Atualizar;
using ServicoJa.Application.UseCases.Servico.Criar;
using ServicoJa.Application.UseCases.Servico.ObterPorId;
using ServicoJa.Application.UseCases.Servico.ObterTodos;

namespace ServicoJa.Controllers;

[ApiController]
[Authorize]
[Route("api/servicos")]
public class ServicoController : ControllerBase
{
    private readonly CriarServicoHandler _criarServicoHandler;
    private readonly ObterServicoPorIdHandler _obterServicoPorIdHandler;
    private readonly ObterTodosServicosHandler _obterTodosServicosHandler;
    private readonly AtualizarServicoHandler _atualizarServicoHandler;
    public ServicoController(
        CriarServicoHandler criarServicoHandler, 
        ObterServicoPorIdHandler obterServicoPorIdHandler, 
        ObterTodosServicosHandler obterTodosServicosHandler,
        AtualizarServicoHandler atualizarServicoHandler)
    {
        _criarServicoHandler = criarServicoHandler;
        _obterServicoPorIdHandler = obterServicoPorIdHandler;
        _obterTodosServicosHandler = obterTodosServicosHandler;
        _atualizarServicoHandler = atualizarServicoHandler;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodosAsync
    (
        [FromQuery] int paginaAtual = 1,
        [FromQuery] int tamanhoPagina = 20
    )
    {
        var idPerfil = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var servicos = await _obterTodosServicosHandler.ExecuteAsync(idPerfil, paginaAtual, tamanhoPagina);

            return Ok(new Response 
            { 
                Sucesso = true, 
                Conteudo = new PagedResponse 
                { 
                    Items = servicos.Servicos, 
                    PaginaAtual = paginaAtual, 
                    TamanhoPagina = tamanhoPagina, 
                    TotalRegistros = servicos.TotalRegistros
                }
            });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha ao encontrar serviços" });
        }
    }

    [HttpGet("{idServico:long}")]
    public async Task<IActionResult> ObterPorId(long idServico)
    {
        var idPerfil = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var servicoResponse = await _obterServicoPorIdHandler.ExecuteAsync(idServico, idPerfil);

            if (servicoResponse is null)
                return NotFound(new Response { Sucesso = false, Mensagem = "Serviço não encontrado", Conteudo = servicoResponse });

            return Ok(new Response { Sucesso = true, Mensagem = "", Conteudo = servicoResponse });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha ao encontrar serviço" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CriarServicoAsync(CriarServicoRequest servicoInput)
    {
        var idPerfil = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var servicoResponse = await _criarServicoHandler.ExecuteAsync(servicoInput, idPerfil);

            return Ok(new Response { Sucesso = true, Mensagem = "", Conteudo = servicoResponse });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha no servidor, tente novamente mais tarde." });
        }
    }

    [HttpPut("{idServico:long}")]
    public async Task<IActionResult> AtualizarServicoAsync(long idServico, AtualizarServicoRequest servicoInput)
    {
        var idPerfil = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var servicoResponse = await _atualizarServicoHandler.ExecuteAsync(idServico, idPerfil, servicoInput);

            if (servicoResponse is null)
                return BadRequest(new Response { Sucesso = false, Mensagem = "Alguma informação é incoêrente. Revise os dados enviados e tente novamente!"});

            return Ok(new Response { Sucesso = true, Mensagem = "", Conteudo = servicoResponse });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha no servidor, tente novamente mais tarde." });
        }
    }
}
