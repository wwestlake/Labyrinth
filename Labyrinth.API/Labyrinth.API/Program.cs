using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Labyrinth.API.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(7253, listenOptions =>
    {
        listenOptions.UseHttps(); // Will use the default dev certificate
    });
});

var serviceAccountKeyJson = builder.Configuration["Firebase:ServiceAccountKey"];
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromJson(serviceAccountKeyJson),
});

// Configure Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy =>
        policy.RequireClaim("role", Role.Administrator.ToString()));
    options.AddPolicy("RequireModeratorRole", policy =>
        policy.RequireClaim("role", Role.Moderator.ToString()));
    options.AddPolicy("RequireOwnerRole", policy =>
        policy.RequireClaim("role", Role.Owner.ToString()));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Labyrinth API", Version = "v1" });

    // Add security definition for Bearer token
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter your token in the format 'Bearer {token}'",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });


    // Add security requirement
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add In-Memory Database
builder.Services.AddDbContext<LabyrinthDbContext>(options =>
    options.UseInMemoryDatabase("MyInMemoryDb"));

// Add custom services and any other dependencies
builder.Services.AddScoped<ICharacterService, CharacterService>();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHttpsRedirection();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Labyrinth API V1");

    });
}


// Seed data after the app is built
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LabyrinthDbContext>();
    SeedData(context);
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Define the SeedData function
void SeedData(LabyrinthDbContext context)
{

}


public partial class Program { }