using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicoJa.Infra.Config;

namespace ServicoJa.Controllers.Identity;

[ApiController]
[Route("api/identity/registrar")]
public class RegistrarController : ControllerBase
{
    public sealed record RegistrarRequest(string Email, string Senha);

    private readonly UserManager<UsuarioIdentity> _userManager;
    
    public RegistrarController(UserManager<UsuarioIdentity> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarAsync(RegistrarRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
            return BadRequest(new { Mensagem = "Email e senha devem ser preenchidos" });

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
        
        return Ok(usuario);
    }
}
