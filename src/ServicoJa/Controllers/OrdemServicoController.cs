using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoJa.Application.UseCases;
using ServicoJa.Application.UseCases.OrdemServico.Criar;
using ServicoJa.Application.UseCases.OrdemServico.ObterPorId;
using ServicoJa.Application.UseCases.OrdemServico.ObterTodosPrestados;
using ServicoJa.Application.UseCases.OrdemServico.ObterTodosSolicitados;

namespace ServicoJa.Controllers;

[ApiController]
[Authorize]
[Route("api/ordem-servicos")]
public class OrdemServicoController : ControllerBase
{
    private readonly CriarOrdemServicoHandler _criarOrdemServicoHandler;
    private readonly ObterOrdemServicoPorIdHandler _ordemServicoPorIdHandler;
    private readonly ObterTodosOrdemServicosPrestadosHandler _obterTodosOrdemServicosPrestadosHandler;
    private readonly ObterTodosOrdemServicosSolicitadosHandler _obterTodosOrdemServicosSolicitadosHandler;
    public OrdemServicoController
    (
        CriarOrdemServicoHandler criarOrdemServicoHandler, 
        ObterOrdemServicoPorIdHandler obterOrdemServicoPorIdHandler,
        ObterTodosOrdemServicosPrestadosHandler obterTodosOrdemServicosPrestadosHandler,
        ObterTodosOrdemServicosSolicitadosHandler obterTodosOrdemServicosSolicitadosHandler
    )
    {
        _criarOrdemServicoHandler = criarOrdemServicoHandler;
        _ordemServicoPorIdHandler = obterOrdemServicoPorIdHandler;
        _obterTodosOrdemServicosPrestadosHandler = obterTodosOrdemServicosPrestadosHandler;
        _obterTodosOrdemServicosSolicitadosHandler = obterTodosOrdemServicosSolicitadosHandler;
    }

    [HttpGet("{idOrdemServico:long}")]
    public async Task<IActionResult> ObterOrdemServicoPorId(long idOrdemServico)
    {
        var idPerfilRequest = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var response = await _ordemServicoPorIdHandler.ExecuteAsync(idOrdemServico, idPerfilRequest);

            if (response == null)
                return BadRequest(new Response { Sucesso = false, Mensagem = "Falha em alguma informação, verifique novamente os dados!" });

            return Ok(new Response { Sucesso = true, Conteudo = response});
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha inesperado servidor, tente novamente mais tarde" });
        }
    }

    [HttpGet("prestados")]
    public async Task<IActionResult> ObterTodosOrdemServicosPrestadosAsync(int paginaAtual = 1, int tamanhoPagina = 25)
    {
        var idPerfilRequest = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var response = await _obterTodosOrdemServicosPrestadosHandler.ExecuteAsync(idPerfilRequest, paginaAtual, tamanhoPagina);

            if (response == null)
                return BadRequest(new Response { Sucesso = false, Mensagem = "Falha em alguma informação, verifique novamente os dados!" });

            return Ok(new Response 
                { 
                    Sucesso = true, 
                    Conteudo = new PagedResponse 
                    {
                        Items = response.OrdemServicos,
                        TotalRegistros = response.TotalRegistros,
                        TamanhoPagina = tamanhoPagina,
                        PaginaAtual = paginaAtual,
                    }
                }
            );
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha inesperado servidor, tente novamente mais tarde" });
        }
    }
    [HttpGet("solicitados")]
    public async Task<IActionResult> ObterTodosOrdemServicosSolicitadosAsync(int paginaAtual = 1, int tamanhoPagina = 25)
    {
        var idPerfilRequest = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var response = await _obterTodosOrdemServicosSolicitadosHandler.ExecuteAsync(idPerfilRequest, paginaAtual, tamanhoPagina);

            if (response == null)
                return BadRequest(new Response { Sucesso = false, Mensagem = "Falha em alguma informação, verifique novamente os dados!" });

            return Ok(new Response
            {
                Sucesso = true,
                Conteudo = new PagedResponse
                {
                    Items = response.OrdemServicos,
                    TotalRegistros = response.TotalRegistros,
                    TamanhoPagina = tamanhoPagina,
                    PaginaAtual = paginaAtual,
                }
            }
            );
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha inesperado servidor, tente novamente mais tarde" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CriarOrdemServicoAsync(CriarOrdemServicoRequest request)
    {
        try
        {
            var response = await _criarOrdemServicoHandler.ExecuteAsync(request);

            if (response == null)
                return BadRequest(new Response { Sucesso = false, Mensagem = "Falha em alguma informação, verifique novamente os dados!" });


            return Ok(new Response { Sucesso = true, Conteudo = response });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha inesperado servidor, tente novamente mais tarde" });
        }
    }
}
