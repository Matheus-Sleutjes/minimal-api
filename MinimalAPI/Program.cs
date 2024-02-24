using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<ILivroService, LivroService>();

string sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection")
                     ??  throw new Exception("Não foi encotrada string de conexão");

builder.Services.AddDbContext<LivrosContext>(options =>
                options.UseNpgsql(sqlConnection));

var app = builder.Build();

// EndPoint Area
    //app.MapGet();
// EndPoint Area
app.Run();

