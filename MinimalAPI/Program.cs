using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ILivroService, LivroService>();

string sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? throw new Exception("Não foi encotrada string de conexão");

builder.Services.AddDbContext<LivrosContext>(options => options.UseNpgsql(sqlConnection));

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
app.MapGet("/{id}", (int id, ILivroService livrosService) => {
    var model = livrosService.Find(id);
    return model != null ? Results.Ok(model) : Results.BadRequest("Não existe entidade com esse id");
});

app.MapPost("/", (Livro model, ILivroService livrosService) => {
    livrosService.Add(model);
    return Results.Ok(model.Id);
});

app.MapGet("/", () => {
    return "It Works";
});
// EndPoint Area
app.Run();

