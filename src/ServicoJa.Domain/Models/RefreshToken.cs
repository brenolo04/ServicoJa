namespace ServicoJa.Domain.Models;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public long IdUsuarioIdentity { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
}
