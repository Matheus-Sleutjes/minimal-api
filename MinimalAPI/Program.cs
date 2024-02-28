using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ILivroService, LivroService>();

builder.Services.AddDbContext<LivrosContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Cria banco e roda migration
var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
var context = serviceScope.ServiceProvider.GetRequiredService<LivrosContext>();
context.Database.Migrate();

// EndPoint Area
app.MapGet("Livros/{id}", (int id, ILivroService livrosService) => {
    var model = livrosService.Find(id);
    return model != null ? Results.Ok(model) : Results.BadRequest("Não existe entidade com esse id");
});

app.MapPost("Livros/", (Livro model, ILivroService livrosService) => {
    livrosService.Add(model);
    livrosService.SaveChanges();
    return Results.Ok(model.Id);
});

app.MapDelete("Livros/{id}", (int id, ILivroService livrosService) => {
    var model = livrosService.Find(id);

    if(model == null)
        return Results.NotFound("Não foi encontrado nenhuma entidade com esse id");

    livrosService.Delete(model);
    livrosService.SaveChanges();
    return Results.Ok("Removido com sucesso!");
});

app.MapPut("Livros/{id}", (int id, Livro request, ILivroService livrosService) => {
    var model = livrosService.Find(id);

    if(model == null)
        return Results.NotFound("Não foi encontrado nenhuma entidade com esse id");

    model.Descricao = request.Descricao;

    livrosService.Update(model);
    livrosService.SaveChanges();
    return Results.Ok(model);
});

app.MapGet("Livros/GetAll", (ILivroService livrosService) => {
    var lista = livrosService.GetAll();
    return Results.Ok(lista);
});
// EndPoint Area
app.Run();

