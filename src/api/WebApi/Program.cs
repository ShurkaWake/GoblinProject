using System.Text.Json.Serialization;
using BusinessLogic.Options;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.

services
    .AddControllersWithViews()
    .AddJsonOptions(options => 
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

string connectionString = builder.Configuration["DbConnectionString"];
services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(connectionString, options =>
    {
        options.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName);
    });
});

services.Configure<SeederOptions>(
    configuration.GetSection(SeederOptions.Section));
services.Configure<JwtOptions>(
    configuration.GetSection(JwtOptions.Section));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

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
