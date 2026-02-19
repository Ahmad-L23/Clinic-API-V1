using ClinicSystem.DAL.Global;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Initialize your DB connection
DataAccessSetting.Initialize(builder.Configuration);

// Add controllers
builder.Services.AddControllers();

// ----------------------
// FluentValidation setup (new way)
// ----------------------
builder.Services.AddFluentValidationAutoValidation(); // Server-side automatic validation
builder.Services.AddFluentValidationClientsideAdapters(); // Optional: for MVC client-side validation

// Register all validators from the DTO assembly
builder.Services.AddValidatorsFromAssembly(Assembly.Load("ClinicSystem.DTOs"));

// OpenAPI / Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
