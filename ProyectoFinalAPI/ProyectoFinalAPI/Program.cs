using ProyectoFinalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<EmailService>();

builder.Services.AddSqlServer<ProyectoContext>(builder.Configuration.GetConnectionString("cnProyecto"));


builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.SetIsOriginAllowed(origin => true)
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
