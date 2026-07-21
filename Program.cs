using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using mapa_asp.net.Data;
using mapa_asp.net.Dto;
using mapa_asp.net.Models;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI + Swagger
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicyCors", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
});

// Connection database
var connectionString =
    Environment.GetEnvironmentVariable("DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicyCors");

app.MapPost("/create", async (MaquinaDto dataDto, AppDbContext appDbContext) =>
{
    var maquina = new Maquina
    {
        Patrimonio = dataDto.Patrimonio,
        Setor = dataDto.Setor,
        Descricao = dataDto.Descricao,
        X = dataDto.X,
        Y = dataDto.Y
    };

    appDbContext.Maquinas.Add(maquina);
    await appDbContext.SaveChangesAsync();

    return Results.Ok(new { message = "Maquina criada com sucesso!" });
})
.WithName("CreateMaquina")
.WithOpenApi();

app.Run();