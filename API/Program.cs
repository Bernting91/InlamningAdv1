using MediatR;
using Application.Books.Queries;
using Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MediatR
builder.Services.AddApplication(); 
builder.Services.AddInfrastructure();

// Register FakeDatabase
builder.Services.AddSingleton<FakeDatabase>();

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