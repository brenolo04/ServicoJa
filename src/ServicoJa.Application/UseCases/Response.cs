namespace ServicoJa.Controllers;

public class Response 
{
    public bool Sucesso { get; set; } 
    public string Mensagem { get; set; } = string.Empty;
    public object? Conteudo { get; set; }
}