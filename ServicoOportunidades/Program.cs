using Microsoft.EntityFrameworkCore;
using ServicoOportunidades.Data;
using ServicoOportunidades.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<OportunidadesContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// HttpClient para integração com outros serviços
builder.Services.AddHttpClient<IntegracaoService>();

// Services
builder.Services.AddScoped<IFichaService, FichaService>();
builder.Services.AddScoped<IntegracaoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Aplicar migrations automaticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OportunidadesContext>();
    context.Database.Migrate();
}

app.Run();
