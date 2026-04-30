using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServicoJa.Infra.Config;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServicoJa.Controllers.Identity;

[ApiController]
[Route("api/identity/login")]
public class LoginController : ControllerBase
{
    public sealed record LoginRequest(string Email, string Senha);

    private readonly UserManager<UsuarioIdentity> _userManager;
    private readonly ServicoJaDbContext _context;
    private readonly IConfiguration _configuration;

    public LoginController(UserManager<UsuarioIdentity> userManager, ServicoJaDbContext context, IConfiguration configuration)
    {
        _userManager = userManager;
        _context = context;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> LogarAsync(LoginRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
            return BadRequest(new { Mensagem = "Email e senha devem ser preenchidos" });

        var usuario = await _userManager.FindByEmailAsync(request.Email);

        if (usuario is null || !await _userManager.CheckPasswordAsync(usuario, request.Senha))
            return BadRequest(new { Mensagem = "Usuário não encontrado ou crendenciais inválidas"});

        return Ok(new {accessToken = GerarToken(usuario) });
    }

    private string GerarToken(UsuarioIdentity usuarioIdentity)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfiguration:SecretKey"]!));
        var credential = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var idPerfil = _context.Perfis.FirstOrDefault(x => x.IdUsuarioIdentity == usuarioIdentity.Id);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, usuarioIdentity.Id.ToString()),
            new("perfilId", idPerfil.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuarioIdentity.Email!),
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = credential,
            Issuer = _configuration["JwtConfiguration:Issuer"],
            Audience = _configuration["JwtConfiguration:Audience"]
        };

        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }
}
