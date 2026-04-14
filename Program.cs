using BookShelfAPI.Data;
using BookShelfAPI.Repositories.Implementions;
using BookShelfAPI.Repositories.Interfaces;
using BookShelfAPI.Services.Implementations;
using BookShelfAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Porta usada pelo Render
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://*:{port}");
}

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL
var connectionString = ResolveConnectionString(builder.Configuration);

builder.Services.AddDbContext<BookShelfContext>(options =>
    options.UseNpgsql(connectionString));

// ---------------- REPOSITORIES ----------------

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IMetaLeituraRepository, MetaLeituraRepository>();
builder.Services.AddScoped<IDesafioAZRepository, DesafioAZRepository>();
builder.Services.AddScoped<ICalendarioMensalRepository, CalendarioMensalRepository>();
builder.Services.AddScoped<IProximaLeitura, ProximaLeituraRepository>();

// ---------------- SERVICES ----------------

builder.Services.AddScoped<IUsuarioServices, UsuarioService>();
builder.Services.AddScoped<ILivroService, LivroService>();
builder.Services.AddScoped<IMetaLeituraService, MetaLeituraService>();
builder.Services.AddScoped<IDesafioAZService, DesafioAZService>();
builder.Services.AddScoped<ICalendarioMensalService, CalendarioMensalService>();
builder.Services.AddScoped<IProximaLeituraService, ProximaLeituraService>();

// ---------------- CORS ----------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// ---------------- SWAGGER ----------------

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookShelf API v1");
    c.RoutePrefix = "swagger";
});

// ---------------- MIDDLEWARE ----------------

app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));


// ---------------- MIGRATIONS ----------------

try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<BookShelfContext>();
        db.Database.Migrate();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao rodar migrations: {ex.Message}");
}

app.Run();


// ---------------- CONEXÃO POSTGRES ----------------

static string ResolveConnectionString(IConfiguration configuration)
{
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (!string.IsNullOrWhiteSpace(databaseUrl))
    {
        return BuildPostgresConnectionStringFromDatabaseUrl(databaseUrl);
    }

    var defaultConnection = configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(defaultConnection))
    {
        throw new InvalidOperationException(
            "Connection string não configurada.");
    }

    return defaultConnection;
}

static string BuildPostgresConnectionStringFromDatabaseUrl(string databaseUrl)
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);

    var builder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port,
        Database = uri.AbsolutePath.Trim('/'),
        Username = userInfo[0],
        Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "",
        SslMode = SslMode.Require
    };

    return builder.ConnectionString;
}