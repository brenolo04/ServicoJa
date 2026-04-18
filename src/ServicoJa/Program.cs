using ServicoJa.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbConfig(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

app.Run();
