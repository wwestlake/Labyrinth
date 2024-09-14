using FirebaseAdmin;
using FluentResults;
using Google.Apis.Auth.OAuth2;
using Labyrinth.API.Common;
using Labyrinth.API.Logging;
using Labyrinth.API.Services;
using Labyrinth.Common;
using Labyrinth.Communication.Chat;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using RulesEngine.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Labyrinth.API.Utilities;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using Labyrinth.Lang;



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
        builder.WithOrigins("https://localhost:5173", "https://localhost:5174", "https://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
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

builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var databaseName = configuration.GetValue<string>("LabyrinthDbName"); // Assumes you store the database name in your config
    return client.GetDatabase(databaseName);
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
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
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
builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto; // Enable polymorphic serialization
            });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(typeof(IBenchmark<>), typeof(Benchmark<>));
builder.Services.AddSingleton(sp =>
    new RoomService(
        sp.GetRequiredService<IMongoClient>(),
        "Labyrinth",
        "Rooms")
);
builder.Services.AddSingleton<ItemService>(sp =>
    new ItemService(
        sp.GetRequiredService<IMongoClient>(),
        "Labyrinth",
        "Items"));
builder.Services.AddSingleton<Func<string, FluentResults.Result<CommandAst>>>(sp =>
{
    // Assuming that `LabLang.LabLang.compileCommand` is the F# function
    return LabLang.Compiler.compileCommand;
});
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<Func<string, Result<CommandAst>>>(sp =>
{
    // Assuming that LabLang.LabLang.compileCommand is the F# function
    return LabLang.Compiler.compileCommand;
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

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(10);
});
builder.Services.AddSingleton<IChatService, ChatService>();
builder.Services.AddScoped<ICommandProcessor, CommandProcessor>();
builder.Services.AddScoped<IRulesEngineService, RulesEngineService>();
builder.Services.AddScoped<IItemPrototypeService, ItemPrototypeService>();
builder.Services.AddScoped<IItemInstanceService, ItemInstanceService>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IFSharpCompilerService, FSharpCompilerService>();


// Register all the compilers
builder.Services.AddScoped<ICompilerFactory, CompilerFactory>();

// Register individual compilers as ILanguageCompiler
builder.Services.AddTransient<ILanguageCompiler, FSharpCompiler>(); // F# Compiler
builder.Services.AddTransient<ILanguageCompiler, CSharpCompiler>(); // C# Compiler
builder.Services.AddTransient<ILanguageCompiler, VBCompiler>(); // C# Compiler

// Register the factory that uses all compilers
builder.Services.AddTransient<Func<SupportedLanguages, ILanguageCompiler>>(sp => key =>
{
    // Get all compilers registered as ILanguageCompiler
    var compilers = sp.GetServices<ILanguageCompiler>();

    return key switch
    {
        SupportedLanguages.CSharp => sp.GetRequiredService<CSharpCompiler>(),
        SupportedLanguages.FSharp => sp.GetRequiredService<FSharpCompiler>(),
        SupportedLanguages.VisualBasic => sp.GetRequiredService<VBCompiler>(),
        _ => throw new NotSupportedException($"Language {key} is not supported.")
    };
});


builder.Services.AddScoped<IPluginService, PluginService>();

var rulesFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "rules.json"); ;
var jsonString = File.ReadAllText(rulesFilePath);


// 9. OpenAI API Configuration
string openAiApiKey = builder.Configuration["OpenAI:ApiKey"];
string botUserName = "HelpBot"; // or read from configuration

// Register HttpClient specifically for ChatBotService
builder.Services.AddHttpClient<ChatBotService>(client =>
{
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
});

// Ensure IHubContext<ChatHub> is registered
builder.Services.AddSingleton<IHubContext<ChatHub>>(sp =>
    sp.GetRequiredService<IHubContext<ChatHub>>());

// Register ChatBotService using a factory method
builder.Services.AddSingleton<ChatBotService>(sp =>
{
    var hubContext = sp.GetRequiredService<IHubContext<ChatHub>>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(nameof(ChatBotService));
    return new ChatBotService(hubContext, httpClient, openAiApiKey, botUserName);
});


var jsonConvertOptions = new JsonSerializerOptions
{
    Converters = { new JsonStringEnumConverter() }
};

// Deserialize the JSON into a list of Workflow objects
var workflows = System.Text.Json.JsonSerializer.Deserialize<List<Workflow>>(jsonString, jsonConvertOptions);

// Initialize the RulesEngine with the workflows
var rulesEngine = new RulesEngine.RulesEngine(workflows.ToArray());

// Register RulesEngine in the DI container
builder.Services.AddSingleton(rulesEngine);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // HTTP port
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS port with default certificate
    });
});
// 9. Application Build and Pipeline Configuration
var app = builder.Build();

var mongoClient = app.Services.GetRequiredService<IMongoClient>();

DatabaseInitializer.InitializeDatabase(mongoClient);


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    using (var scope = app.Services.CreateScope())
    {
        var roomService = scope.ServiceProvider.GetRequiredService<RoomService>();
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

app.UseHttpsRedirection();
app.UseWebSockets();
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
    _ = endpoints.MapHub<ChatHub>("/chat").RequireCors("AllowFrontend"); ;
});
app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application Started");


app.Run();

public partial class Program { }
