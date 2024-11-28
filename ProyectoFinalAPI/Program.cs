using Hangfire;
using ProyectoFinalAPI;
using ProyectoFinalAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configura Hangfire
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });


builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("cnProyecto")));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<CarritoService>();

builder.Services.AddSqlServer<ProyectoContext>(builder.Configuration.GetConnectionString("cnProyecto"));


builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.WithOrigins("http://localhost:4200", "http://localhost:5173")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
    });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5194);
});

var app = builder.Build();
// Configura el middleware
app.UseHangfireDashboard(); // A�ade el panel de control de Hangfire
app.UseHangfireServer(); // Inicia el servidor de Hangfire


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NuevaPolitica");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
recurringJobManager.AddOrUpdate<PromocionesRandomService>(
    "ActualizarPromocionesRandom",
    service => service.EjecutarPromocionesAleatorias(),
    Cron.Hourly);

var recurringJobManager2 = app.Services.GetRequiredService<IRecurringJobManager>();
recurringJobManager2.AddOrUpdate<CarritoService>(
    "EnviarRecordatoriosCarritos",
    service => service.EnviarRecordatoriosCarritosAsync(),
    Cron.MinuteInterval(1440)); 
    // Cron.MinuteInterval(5)); 
app.Run();