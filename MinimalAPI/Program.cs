using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ILivroService, LivroService>();

builder.Services.AddDbContext<LivrosContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    var model = _livrosService.Find(id);
    return model != null ? Results.Ok(model) : Results.BadRequest("Não existe entidade com esse id");
});

app.MapPost("EFPost/", (Livro model, ILivroService _livrosService) =>
{
    _livrosService.Add(model);
    return Results.Ok(model.Id);
});

#endregion

#region SQL 
app.MapPost("SQLPost", (Livro model, ILivroService _livrosService) =>
{
    _livrosService.AddBySql(model, builder.Configuration.GetConnectionString("DefaultConnection"));
    return Results.Ok();
});
#endregion
app.Run();

