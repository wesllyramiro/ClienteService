using System.Data;
using Microsoft.Data.SqlClient;
using ClienteService.Services;
using ClienteService.Repositories;

var builder = WebApplication.CreateBuilder(args);

var dockerSettings = Path.Combine(AppContext.BaseDirectory, "appsettings.Docker.json");
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" || File.Exists(dockerSettings))
{
    builder.Configuration.AddJsonFile(dockerSettings, optional: true, reloadOnChange: true);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IDbConnection>(sp => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteServiceImpl>();

var app = builder.Build();
try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var scriptPath = Path.Combine(AppContext.BaseDirectory, "scripts", "create_tables_and_seed.sql");
    if (File.Exists(scriptPath))
    {
        var script = File.ReadAllText(scriptPath);
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = script;
        cmd.ExecuteNonQuery();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"DB init skipped: {ex.Message}");
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();