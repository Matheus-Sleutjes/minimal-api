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

// EndPoint Area
app.MapGet("Livros/{id}", (int id, ILivroService _livrosService) =>
{
    var model = _livrosService.Find(id);
    return model != null ? Results.Ok(model) : Results.BadRequest("Não existe entidade com esse id");
});

app.MapPost("Livros/", (Livro model, ILivroService _livrosService) =>
{
    _livrosService.Add(model);
    _livrosService.SaveChanges();
    return Results.Ok(model.Id);
});

app.MapDelete("Livros/{id}", (int id, ILivroService _livrosService) =>
{
    var model = _livrosService.Find(id);

    if (model == null)
        return Results.NotFound("Não foi encontrado nenhuma entidade com esse id");

    _livrosService.Delete(model);
    _livrosService.SaveChanges();
    return Results.Ok("Removido com sucesso!");
});

app.MapPut("Livros/{id}", (int id, Livro request, ILivroService _livrosService) =>
{
    var model = _livrosService.Find(id);

    if (model == null)
        return Results.NotFound("Não foi encontrado nenhuma entidade com esse id");

    model.Descricao = request.Descricao;

    _livrosService.Update(model);
    _livrosService.SaveChanges();
    return Results.Ok(model);
});

app.MapGet("Livros/GetAll", (ILivroService _livrosService) =>
{
    var lista = _livrosService.GetAll();
    return Results.Ok(lista);
});
// EndPoint Area
app.Run();

