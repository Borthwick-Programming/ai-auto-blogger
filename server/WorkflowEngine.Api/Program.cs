using WorkflowEngine.Domain.Models;
using WorkflowEngine.Runtime.Interfaces;
using WorkflowEngine.Runtime.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add support for MVC-style controllers
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Add support for Swagger/OpenAPI documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Create an in-memory node registry to hold all available node types.
// This is where we define the behavior and configuration schema of nodes
var nodeRegistry = new InMemoryNodeRegistry();

// Register a core node type: HTTP Request
// This node allows external API calls with configurable method and URL
nodeRegistry.Register(new NodeDefinition(
    Id: "http-request",
    Name: "HTTP Request",
    Description: "Calls an external API.",
    NodeType: "http-request",
    ConfigurationSchemaJson: """
    {
        "type": "object",
        "properties": {
            "url": { "type": "string" },
            "method": { "type": "string", "enum": ["GET", "POST", "PUT", "DELETE"] }
        },
        "required": ["url", "method"]
    }
    """,
    Inputs: new(),
    Outputs: new() { new PortDefinition("response", "object") }
));

// Register the node registry as a singleton service so it can be injected via DI
builder.Services.AddSingleton<INodeRegistry>(nodeRegistry);


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
