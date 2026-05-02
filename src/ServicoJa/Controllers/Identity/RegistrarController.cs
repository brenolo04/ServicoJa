using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicoJa.Domain.Models;
using ServicoJa.Infra.Config;

namespace ServicoJa.Controllers.Identity;

[ApiController]
[Route("api/identity/registrar")]
public class RegistrarController : ControllerBase
{
    public sealed record RegistrarRequest(string Email, string Senha);

    private readonly UserManager<UsuarioIdentity> _userManager;
    private readonly ServicoJaDbContext _servicoJaDbContext;
    
    public RegistrarController(UserManager<UsuarioIdentity> userManager, ServicoJaDbContext dbContext)
    {
        _userManager = userManager;
        _servicoJaDbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarAsync(RegistrarRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
            return BadRequest(new { Mensagem = "Email e senha devem ser preenchidos" });

        using var transaction = await _servicoJaDbContext.Database.BeginTransactionAsync();

        try
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not null)
                return BadRequest(new { Mensagem = "Usuário já cadastrado com esse email, tente outro." });

            var usuario = new UsuarioIdentity
            {
                UserName = request.Email,
                Email = request.Email
            };

            var resultado = await _userManager.CreateAsync(usuario, request.Senha);

            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            var perfil = new Perfil(usuario.Id);

            _servicoJaDbContext.Perfis.Add(perfil);
            await _servicoJaDbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new { Mensagem = "Usuário criado" });
        }
        catch 
        {
            transaction.Rollback();
            return StatusCode(500, new { Mensagem = "Erro ao tentar criar usuário" });
        }
    }
}
