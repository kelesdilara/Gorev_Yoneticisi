using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using piton_taskmanagement_api.Context;
using piton_taskmanagement_api.Repositories.Implementations;
using piton_taskmanagement_api.Repositories.Interfaces;
using piton_taskmanagement_api.Services.Implementations;
using piton_taskmanagement_api.Services.Interfaces;
using piton_taskmanagement_api.Settings;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ------------------------
// CONFIGURATION BINDINGS
// ------------------------

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// ------------------------
// DEPENDENCY INJECTION
// ------------------------

builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();

// User Module
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IJwtService, JwtService>();

// Task Module
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

// ------------------------
// JWT AUTHENTICATION
// ------------------------

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };
});

// ------------------------
// CONTROLLERS & JSON CONFIG
// ------------------------

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Enum'ları string olarak serialize et
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // Case insensitive enum parsing için
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        // Property naming policy
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // PascalCase korumak için
    });

// ------------------------
// SWAGGER (JWT DESTEKLİ)
// ------------------------

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Görev Yönetimi API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Örn: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});

// ------------------------
// BUILD PIPELINE
// ------------------------

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication(); // JWT Middleware
app.UseAuthorization();

app.MapControllers();

app.Run();