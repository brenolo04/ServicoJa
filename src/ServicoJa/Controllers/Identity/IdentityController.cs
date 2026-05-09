using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using ServicoJa.Domain.Models;
using ServicoJa.Infra.Config;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace ServicoJa.Controllers.Identity;

[ApiController]
[Route("api/identity")]
public class IdentityController : ControllerBase
{
    public sealed record RegistrarRequest(string Nome, string Email, string Senha);
    public sealed record LoginRequest(string Email, string Senha);
    public sealed record LoginResponse(string AccessToken, string RefreshToken);
    public sealed record RefreshTokenRequest(string RefreshToken);

    private readonly UserManager<UsuarioIdentity> _userManager;
    private readonly ServicoJaDbContext _context;
    private readonly IConfiguration _configuration;

    public IdentityController(UserManager<UsuarioIdentity> userManager, ServicoJaDbContext context, IConfiguration configuration)
    {
        _userManager = userManager;
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> RegistrarAsync(RegistrarRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
            return BadRequest(new { Mensagem = "Email e senha devem ser preenchidos" });

        using var transaction = await _context.Database.BeginTransactionAsync();

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

            var perfil = new Perfil(usuario.Id, request.Nome);

            _context.Perfis.Add(perfil);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new { Mensagem = "Usuário criado" });
        }
        catch
        {
            transaction.Rollback();
            return StatusCode(500, new Response { Sucesso = true, Mensagem = "Erro ao tentar criar usuário"});
        }
        finally { await transaction.DisposeAsync(); }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LogarAsync(LoginRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
                return BadRequest(new { Mensagem = "Email e senha devem ser preenchidos" });

            var usuarioIdentity = await _userManager.FindByEmailAsync(request.Email);

            if (usuarioIdentity is null || !await _userManager.CheckPasswordAsync(usuarioIdentity, request.Senha))
                return BadRequest(new { Mensagem = "Usuário não encontrado ou crendenciais inválidas" });

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                IdUsuarioIdentity = usuarioIdentity.Id,
                Token = GerarRefreshToken(),
                ExpiresOnUtc = DateTime.UtcNow.AddDays(7),
            };

            await _context.RefreshTokens.Where(x => x.IdUsuarioIdentity == usuarioIdentity.Id).ExecuteDeleteAsync();
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new Response
            {
                Sucesso = true,
                Conteudo = new LoginResponse(GerarToken(usuarioIdentity), refreshToken.Token)
            });
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha no servidor ao tentar se logar" });
        }
        finally { await transaction.DisposeAsync(); }
        
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var refreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken);

            if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
                return BadRequest(new Response { Sucesso = false, Mensagem = "Falha ao tentar se reautenticar. Será necessário se autenticar novamente!" });

            var usuarioIdentity = _context.Users.First(x => x.Id == refreshToken.IdUsuarioIdentity);

            var novoRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = GerarRefreshToken(),
                IdUsuarioIdentity = usuarioIdentity.Id,
                ExpiresOnUtc = DateTime.UtcNow.AddDays(7),
            };

            await _context.RefreshTokens.Where(x => x.IdUsuarioIdentity == usuarioIdentity.Id).ExecuteDeleteAsync();
            await _context.RefreshTokens.AddAsync(novoRefreshToken);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new Response { Sucesso = true, Conteudo = new LoginResponse(GerarToken(usuarioIdentity), novoRefreshToken.Token )});
        }
        catch
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new Response { Sucesso = false, Mensagem = "Falha no servidor ao tentar se reautenticar!" });
        }
        finally { await transaction.DisposeAsync(); }
    }

    private string GerarToken(UsuarioIdentity usuarioIdentity)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfiguration:SecretKey"]!));
        var credential = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var idPerfil = _context.Perfis.FirstOrDefault(x => x.IdUsuarioIdentity == usuarioIdentity.Id);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, usuarioIdentity.Id.ToString()),
            new("idPerfil", idPerfil.Id.ToString()),
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

    private string GerarRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
}
