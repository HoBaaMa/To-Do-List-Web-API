using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using To_Do_List_Web_API.Data;
using To_Do_List_Web_API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(); // To support JsonPatch
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TodoDbContext>(o => o.UseSqlServer(connectionString));

builder.Services.AddScoped<ITodoRepository, TodoRepository>();

// Configure Serilog
//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .WriteTo.Console()
//    .WriteTo.File("logs/todolist-.log", rollingInterval: RollingInterval.Day)
//    .CreateLogger();

builder.Host.UseSerilog((context, services, configuration) =>
 {
     configuration.ReadFrom.Configuration(context.Configuration);
     configuration.ReadFrom.Services(services);
 });

// Replace the default logging provider with Serilog
//builder.Host.UseSerilog(); // This ensures Serilog handles all logging


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

app.Run();
