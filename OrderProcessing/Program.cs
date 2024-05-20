using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OrderProcessing.Data;
using OrderProcessing.RabbitMQS;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var server = Environment.GetEnvironmentVariable("DB_SERVER");
var database = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
var connectionString = $"Server={server},1433;Database={database};User={user};Password={password};TrustServerCertificate=True";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Order_Db>(options =>
 options.UseSqlServer(connectionString, options =>
 {
     options.EnableRetryOnFailure(maxRetryCount: 9, maxRetryDelay: TimeSpan.FromSeconds(2),
         errorNumbersToAdd: null);
 })
);

ServiceLifetime.Singleton); // Tilføj denne linje for at angive levetiden som singleton
builder.Services.AddSingleton<IRabbitMQ, RabbitMQUnti>();
builder.Services.AddHostedService<RabbitMqServicecs>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-feeu3ze3mjv64zbn.eu.auth0.com/";
    options.Audience = "https://www.eaau2024.com";
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Migrate database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Order_Db>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<Order_Db>();
    db.Database.EnsureCreated();

}
    app.UseSwagger();
    app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// Run the application
app.Run();
