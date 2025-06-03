using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Reflection;
using WorkflowEngine.Api.Auth;
using WorkflowEngine.Api.Configuration;
using WorkflowEngine.Api.Services;
using WorkflowEngine.Core.Interfaces;
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
              .AllowAnyMethod()
              .AllowCredentials()); // Allow credentials if needed
});

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Add support for MVC-style controllers
builder.Services.AddControllers(options =>
{
    // Apply [Authorize] to all controllers by default
    // This will require authentication for all endpoints unless overridden; [AllowAnonymous] decorated endpoints stay open
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Add support for Swagger/OpenAPI documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Resolve the XML comments file
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Create an in-memory node registry to hold all available node types.
// Define the behavior and configuration schema of nodes

builder.Services.AddNodeDefinitions(builder.Configuration, builder.Environment);

builder.Services.AddPersistence(builder.Configuration); //persistence setup

//For dev, we nmeed a call with an inline, environment-aware block
//builder.Services.AddAuthenticationServices(builder.Configuration); //old line
if (builder.Environment.IsDevelopment())
{
    // In Development, use a fake "Test" scheme that always authenticates you as the dev user
    builder.Services
           .AddAuthentication("Dev")
           .AddScheme<AuthenticationSchemeOptions, DevAuthHandler>(
                "Dev", options => { })
               .Services.AddSingleton<IConfiguration>(builder.Configuration);
}
else
{
    // In Production, use real Windows Negotiate
    builder.Services
           .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
           .AddNegotiate();
}
builder.Services.AddAuthorization();


builder.Services.AddApplicationServices(); //application services setup

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
app.UseCors("AllowViteClient");
app.UseAuthentication();
app.UseAuthorization();

// Enable routing to controllers (like NodeDefinitionsController)
app.MapControllers();

app.Run();
