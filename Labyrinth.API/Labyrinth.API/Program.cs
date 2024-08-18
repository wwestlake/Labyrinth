using FirebaseAdmin;
using FluentResults;
using Google.Apis.Auth.OAuth2;
using Labyrinth.API.Common;
using Labyrinth.API.Entities.Rules;
using Labyrinth.API.Logging;
using Labyrinth.API.Services;
using Labyrinth.API.Utilities;
using Labyrinth.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// 1. Logging Configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton<ILoggerProvider, MongoDBLoggerProvider>(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDB:DatabaseName"];
    return new MongoDBLoggerProvider(mongoClient, databaseName, "Logs");
});

// 2. CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// 3. Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/lagdaemon-game-authentication";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://securetoken.google.com/lagdaemon-game-authentication",
            ValidateAudience = true,
            ValidAudience = "lagdaemon-game-authentication",
            ValidateLifetime = true
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated for user: " + context.Principal.Identity.Name);
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine("OnChallenge error: " + context.Error + ", " + context.ErrorDescription);
                return Task.CompletedTask;
            }
        };
    });

// 4. Authorization Configuration
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewOwnProfile", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "user_id" && c.Value == context.Resource.ToString()) ||
            context.User.IsInRole(Role.Administrator.ToString()) ||
            context.User.IsInRole(Role.Owner.ToString())
        ));

    options.AddPolicy("CanModifyOwnProfile", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "user_id" && c.Value == context.Resource.ToString()) ||
            context.User.IsInRole(Role.Administrator.ToString()) ||
            context.User.IsInRole(Role.Owner.ToString())
        ));

    options.AddPolicy("CanViewAllUsers", policy =>
        policy.RequireRole(Role.Moderator.ToString(), Role.Administrator.ToString(), Role.Owner.ToString()));

    options.AddPolicy("CanModifyAnyUser", policy =>
        policy.RequireRole(Role.Administrator.ToString(), Role.Owner.ToString()));

    options.AddPolicy("CanBanUsers", policy =>
        policy.RequireRole(Role.Administrator.ToString(), Role.Owner.ToString()));

    options.AddPolicy("RequireAdministratorRole", policy =>
        policy.RequireClaim("role", Role.Administrator.ToString()));
    options.AddPolicy("RequireModeratorRole", policy =>
        policy.RequireClaim("role", Role.Moderator.ToString()));
    options.AddPolicy("RequireOwnerRole", policy =>
        policy.RequireClaim("role", Role.Owner.ToString()));
});

// 5. Firebase Configuration
var serviceAccountKeyJson = builder.Configuration["Firebase:ServiceAccountKey"];
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromJson(serviceAccountKeyJson),
});

// 6. MongoDB Configuration
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var connectionString = builder.Configuration["MongoDB:ConnectionString"];
    return new MongoClient(connectionString);
});

// 7. Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Labyrinth API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Enter only the token here.",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

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

// 8. Service Configuration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(typeof(IBenchmark<>), typeof(Benchmark<>));
builder.Services.AddSingleton(sp =>
    new RoomService(
        sp.GetRequiredService<IMongoClient>(),
        "Labyrinth",
        "rooms")
);
builder.Services.AddSingleton<ItemService>(sp =>
    new ItemService(
        sp.GetRequiredService<IMongoClient>(),
        "Labyrinth",
        "items"));
builder.Services.AddSingleton<Func<string, FluentResults.Result<CommandAst>>>(sp =>
{
    // Assuming that `LabLang.LabLang.compileCommand` is the F# function
    return LabLang.LabLang.compileCommand;
});
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<Func<string, Result<CommandAst>>>(sp =>
{
    // Assuming that LabLang.LabLang.compileCommand is the F# function
    return LabLang.LabLang.compileCommand;
});
builder.Services.AddScoped<ICommandCompilerService, CommandCompilerService>();
builder.Services.AddSingleton<IClaimsTransformation>(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDB:DatabaseName"];
    var usersCollectionName = "ApplicationUsers"; // Assuming this is the collection name

    return new ClaimsTransformer(mongoClient, databaseName, usersCollectionName);
});
builder.Services.AddScoped<ICommandProcessor, CommandProcessor>();

var rulesFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "rules.json");
var jsonString = File.ReadAllText(rulesFilePath);
var rules = JsonSerializer.Deserialize<Dictionary<string, List<Rule>>>(jsonString);


// 9. Application Build and Pipeline Configuration
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    using (var scope = app.Services.CreateScope())
    {
        var roomService = scope.ServiceProvider.GetRequiredService<RoomService>();
        await roomService.SeedRoomsAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Labyrinth API V1");
    });
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application Started");

app.Run();

public partial class Program { }
