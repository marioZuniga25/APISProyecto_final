using ProyectoFinalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSqlServer<ProyectoContext>(builder.Configuration.GetConnectionString("cnProyecto"));


builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.SetIsOriginAllowed(origin => true) // Permite cualquier origen temporalmente
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials(); // Si estás usando cookies o autenticación con credenciales
    });
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5194); // Escucha en todas las interfaces de red
});

var app = builder.Build();

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

app.Run();
