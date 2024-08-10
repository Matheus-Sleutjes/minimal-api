using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Entities;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddTransient<ILivroService, LivroService>();

builder.Services.AddDbContext<LivrosContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//Cria banco e roda migration
var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
var context = serviceScope.ServiceProvider.GetRequiredService<LivrosContext>();
context.Database.Migrate();

#region EntityFramework

app.MapGet("EFGet/{id}", (int id, ILivroService _livrosService) =>
{
    var entity = _livrosService.GetByEf(id);
    return entity != null ? Results.Ok(entity) : Results.BadRequest("Não existe entidade com esse id");
});

app.MapPost("EFPost/", (Livro model, ILivroService _livrosService) =>
{
    _livrosService.AddByEf(model);
    return Results.Ok();
});

#endregion

#region SQL 

app.MapGet("SQLGet/{id}", (int id, ILivroService _livrosService) =>
{
    var entity = _livrosService.GetBySql(id, connectionString);
    return entity != null ? Results.Ok(entity) : Results.BadRequest("Não existe entidade com esse id");
});

app.MapPost("SQLPost", (Livro model, ILivroService _livrosService) =>
{
    _livrosService.AddBySql(model, connectionString);
    return Results.Ok();
});

#endregion
app.Run();

