using ClinicSystem.DAL.Global;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Initialize your DB connection
// ----------------------
DataAccessSetting.Initialize(builder.Configuration);

// ----------------------
// Add services to the container
// ----------------------
builder.Services.AddControllers();

// FluentValidation setup
builder.Services.AddFluentValidationAutoValidation();       // Server-side automatic validation
builder.Services.AddFluentValidationClientsideAdapters();   // Optional: for client-side validation
builder.Services.AddValidatorsFromAssembly(Assembly.Load("ClinicSystem.DTOs"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------------------
// Build the app
// ----------------------
var app = builder.Build();

// ----------------------
// Configure middleware
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();