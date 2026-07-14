using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoJa.Application.UseCases;
using ServicoJa.Application.UseCases.Servico.Ativar;
using ServicoJa.Application.UseCases.Servico.Atualizar;
using ServicoJa.Application.UseCases.Servico.Criar;
using ServicoJa.Application.UseCases.Servico.Inativar;
using ServicoJa.Application.UseCases.Servico.ObterPorId;
using ServicoJa.Application.UseCases.Servico.ObterTodos;
using ServicoJa.Domain.Errors;
using ServicoJa.Domain.Results;

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
    private readonly InativarServicoHandler _inativarServicoHandler;
    private readonly AtivarServicoHandler _ativarServicoHandler;
    public ServicoController(CriarServicoHandler criarServicoHandler, 
        ObterServicoPorIdHandler obterServicoPorIdHandler, 
        ObterTodosServicosHandler obterTodosServicosHandler,
        AtualizarServicoHandler atualizarServicoHandler,
        InativarServicoHandler inativarServicoHandler,
        AtivarServicoHandler ativarServicoHandler
    )
    {
        _criarServicoHandler = criarServicoHandler;
        _obterServicoPorIdHandler = obterServicoPorIdHandler;
        _obterTodosServicosHandler = obterTodosServicosHandler;
        _atualizarServicoHandler = atualizarServicoHandler;
        _inativarServicoHandler = inativarServicoHandler;
        _ativarServicoHandler = ativarServicoHandler;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodosAsync(
        [FromQuery] int paginaAtual = 1,
        [FromQuery] int tamanhoPagina = 20
    )
    {
        var idPerfil = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var result = await _obterTodosServicosHandler.ExecuteAsync(idPerfil, paginaAtual, tamanhoPagina);

            if(result.Reasons.OfType<ListaVaziaSuccess>().Any())
                return Ok(new Response
                {
                    Sucesso = true,
                    Mensagem = result.Successes.FirstOrDefault()!.Message,
                    Conteudo = new PagedResponse
                    {
                        Items = Array.Empty<object>(),
                        PaginaAtual = paginaAtual,
                        TamanhoPagina = tamanhoPagina,
                        TotalRegistros = 0
                    }
                });
            

            return Ok(new Response 
            { 
                Sucesso = true, 
                Conteudo = new PagedResponse 
                { 
                    Items = result.Value.Servicos, 
                    PaginaAtual = paginaAtual, 
                    TamanhoPagina = tamanhoPagina, 
                    TotalRegistros = result.Value.TotalRegistros
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
            var result = await _obterServicoPorIdHandler.ExecuteAsync(idServico, idPerfil);

            if (result.Reasons.OfType<EntidadeVaziaError>().Any())
                return NotFound(new Response { Sucesso = false, Mensagem = result.Errors.FirstOrDefault()!.Message });

            return Ok(new Response { Sucesso = true, Conteudo = result.Value });
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
            var result = await _criarServicoHandler.ExecuteAsync(servicoInput, idPerfil);

            return Ok(new Response { Sucesso = true, Mensagem = "", Conteudo = result.Value });
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
            var result = await _atualizarServicoHandler.ExecuteAsync(idServico, idPerfil, servicoInput);

            if (result.IsFailed)
                return NotFound(new Response { Sucesso = false, Mensagem = result.Errors.FirstOrDefault()!.Message });

            return Ok(new Response { Sucesso = true, Mensagem = "", Conteudo = result.Value });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha no servidor, tente novamente mais tarde." });
        }
    }

    [HttpPatch("{idServico:long}/inativar")]
    public async Task<IActionResult> InativarServicoAsync(long idServico)
    {
        var idPerfil = long.Parse(User.FindFirst("idPerfil")!.Value);
        try
        {
            var result = await _inativarServicoHandler.ExecuteAsync(idServico, idPerfil);
            if (result.IsFailed)
                return NotFound(new Response { Sucesso = false, Mensagem = result.Errors.FirstOrDefault()!.Message });
            return Ok(new Response { Sucesso = true, Conteudo = result.Value });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha no servidor, tente novamente mais tarde." });
        }
    }

    [HttpPatch("{idServico:long}/ativar")]
    public async Task<IActionResult> AtivarServicoAsync(long idServico)
    {
        var idPerfil = long.Parse(User.FindFirst("idPerfil")!.Value);
        try
        {
            var result = await _ativarServicoHandler.ExecuteAsync(idServico, idPerfil);
            if (result.IsFailed)
                return NotFound(new Response { Sucesso = false, Mensagem = result.Errors.FirstOrDefault()!.Message });

            return Ok(new Response { Sucesso = true, Conteudo = result.Value });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha no servidor, tente novamente mais tarde." });
        }
    }
}
