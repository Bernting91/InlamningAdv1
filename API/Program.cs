using Application;
using Infrastructure.Database;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
byte[] secretkey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidAudience = jwtSettings["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretkey)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", Policy =>
    {
        Policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        Policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Robert Bernting", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Authorize with your bearer token that generates when you login",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Enable annotations
    c.EnableAnnotations();
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MediatR
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection")!);

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