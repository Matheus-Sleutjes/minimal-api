var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ILivroService, LivroService>();

string sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection")
                     ??  throw new Exception("Não foi encotrada string de conexão");

var app = builder.Build();


app.Run();

