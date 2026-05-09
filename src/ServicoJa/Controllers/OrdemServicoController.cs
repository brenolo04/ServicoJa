using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoJa.Application.UseCases;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar.Aprovar;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar.Cancelar;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar.Endereco;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar.Executar;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar.Finalizar;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar.SolicitanteAnonimo;
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
    private readonly AprovarOrdemServicoHandler _aprovarOrdemServicoHandler;
    private readonly ExecutarOrdemServicoHandler _executarOrdemServicoHandler;
    private readonly FinalizarOrdemServicoHandler _finalizarOrdemServicoHandler;
    private readonly CancelarOrdemServicoHandler _cancelarOrdemServicoHandler;
    private readonly SolicitanteAnonimoHandler _solicitanteAnonimoHandler;
    private readonly EnderecoOrdemServicoHandler _enderecoOrdemServicoHandler;
    public OrdemServicoController
    (
        CriarOrdemServicoHandler criarOrdemServicoHandler, 
        ObterOrdemServicoPorIdHandler obterOrdemServicoPorIdHandler,
        ObterTodosOrdemServicosPrestadosHandler obterTodosOrdemServicosPrestadosHandler,
        ObterTodosOrdemServicosSolicitadosHandler obterTodosOrdemServicosSolicitadosHandler,
        AprovarOrdemServicoHandler aprovarOrdemServicoHandler,
        ExecutarOrdemServicoHandler executarOrdemServicoHandler,
        FinalizarOrdemServicoHandler finalizarOrdemServicoHandler,
        CancelarOrdemServicoHandler cancelarOrdemServicoHandler,
        SolicitanteAnonimoHandler solicitanteAnonimoHandler,
        EnderecoOrdemServicoHandler enderecoOrdemServicoHandler
    )
    {
        _criarOrdemServicoHandler = criarOrdemServicoHandler;
        _ordemServicoPorIdHandler = obterOrdemServicoPorIdHandler;
        _obterTodosOrdemServicosPrestadosHandler = obterTodosOrdemServicosPrestadosHandler;
        _obterTodosOrdemServicosSolicitadosHandler = obterTodosOrdemServicosSolicitadosHandler;
        _aprovarOrdemServicoHandler = aprovarOrdemServicoHandler;
        _executarOrdemServicoHandler = executarOrdemServicoHandler;
        _finalizarOrdemServicoHandler = finalizarOrdemServicoHandler;
        _cancelarOrdemServicoHandler = cancelarOrdemServicoHandler;
        _solicitanteAnonimoHandler = solicitanteAnonimoHandler;
        _enderecoOrdemServicoHandler = enderecoOrdemServicoHandler;
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

    [HttpPatch("{idOrdemServico:long}/aprovar")]
    public async Task<IActionResult> AprovarOrdemServicoAsync(long idOrdemServico)
    {
        var idPerfilRequest = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var response = await _aprovarOrdemServicoHandler.ExecuteAsync(idOrdemServico, idPerfilRequest);

            if (response == null)
                return BadRequest(new Response { Sucesso = false, Mensagem = "Falha em alguma informação, verifique novamente os dados!" });

            return Ok(new Response { Sucesso = true, Conteudo = response });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha inesperado servidor, tente novamente mais tarde" });
        }
    }

    [HttpPatch("{idOrdemServico:long}/executar")]
    public async Task<IActionResult> ExecutarOrdemServicoAsync(long idOrdemServico)
    {
        var idPerfilRequest = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var response = await _executarOrdemServicoHandler.ExecuteAsync(idOrdemServico, idPerfilRequest);

            if (response == null)
                return BadRequest(new Response { Sucesso = false, Mensagem = "Falha em alguma informação, verifique novamente os dados!" });

            return Ok(new Response { Sucesso = true, Conteudo = response });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha inesperado servidor, tente novamente mais tarde" });
        }
    }

    [HttpPatch("{idOrdemServico:long}/finalizar")]
    public async Task<IActionResult> FinalizarOrdemServicoAsync(long idOrdemServico)
    {
        var idPerfilRequest = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var response = await _finalizarOrdemServicoHandler.ExecuteAsync(idOrdemServico, idPerfilRequest);

            if (response == null)
                return BadRequest(new Response { Sucesso = false, Mensagem = "Falha em alguma informação, verifique novamente os dados!" });

            return Ok(new Response { Sucesso = true, Conteudo = response });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha inesperado servidor, tente novamente mais tarde" });
        }
    }

    [HttpPatch("{idOrdemServico:long}/cancelar")]
    public async Task<IActionResult> CancelarOrdemServicoAsync(long idOrdemServico)
    {
        var idPerfilRequest = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var response = await _cancelarOrdemServicoHandler.ExecuteAsync(idOrdemServico, idPerfilRequest);

            if (response == null)
                return BadRequest(new Response { Sucesso = false, Mensagem = "Falha em alguma informação, verifique novamente os dados!" });

            return Ok(new Response { Sucesso = true, Conteudo = response });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha inesperado servidor, tente novamente mais tarde" });
        }
    }

    [HttpPatch("{idOrdemServico:long}/solicitante-anonimo")]
    public async Task<IActionResult> AtualizarSolicitanteAnonimoAsync(long idOrdemServico, SolicitanteAnonimoRequest request)
    {
        var idPerfilRequest = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var response = await _solicitanteAnonimoHandler.ExecuteAsync(idOrdemServico, idPerfilRequest, request);

            if (response == null)
                return BadRequest(new Response { Sucesso = false, Mensagem = "Falha em alguma informação, verifique novamente os dados!" });

            return Ok(new Response { Sucesso = true, Conteudo = response });
        }
        catch
        {
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha inesperado servidor, tente novamente mais tarde" });
        }
    }

    [HttpPatch("{idOrdemServico:long}/endereco")]
    public async Task<IActionResult> AtualizarEnderecoAsync(long idOrdemServico, EnderecoOrdemServicoRequest request)
    {
        var idPerfilRequest = long.Parse(User.FindFirst("idPerfil")!.Value);

        try
        {
            var response = await _enderecoOrdemServicoHandler.ExecuteAsync(idOrdemServico, idPerfilRequest, request);

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
