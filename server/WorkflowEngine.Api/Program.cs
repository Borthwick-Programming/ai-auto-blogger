using System;
using System.Reflection;
using WorkflowEngine.Api.Configuration;
using WorkflowEngine.Domain.Models;
using WorkflowEngine.Runtime.Interfaces;
using WorkflowEngine.Runtime.Services;



var builder = WebApplication.CreateBuilder(args);

var dataPath = Path.Combine(builder.Environment.ContentRootPath, "Data");
Directory.CreateDirectory(dataPath);

//cors
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteClient", policy =>
        policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Add services to the container.

// Add support for MVC-style controllers
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Add support for Swagger/OpenAPI documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Create an in-memory node registry to hold all available node types.
// Define the behavior and configuration schema of nodes
//builder.Services.AddNodeRegistry(); //node setups

//builder.Services.AddNodeDefinitions(builder.Configuration, builder.Environment);//works by itself
builder.Services.AddNodeDefinitions(builder.Configuration, builder.Environment);

builder.Services.AddPersistence(builder.Configuration); //persistence setup

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Serve Swagger UI and static assets like JavaScript, CSS
    app.UseStaticFiles();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Disable forced HTTPS for dev
//app.UseHttpsRedirection(); 

app.UseAuthorization();

// Enable routing to controllers (like NodeDefinitionsController)
app.MapControllers();

app.Run();
