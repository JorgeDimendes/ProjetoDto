using Microsoft.EntityFrameworkCore;
using ProjetoDto.Api.Data;
using ProjetoDto.Api.Service.Manual;
using ProjetoDto.Api.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//SqLite
builder.Services.AddDbContext<AppDbContext>(context =>
    context.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//Service
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IProdutoMapsterService, ProdutoMapsterService>();

// Registra o Mapster

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();