// H�r skapas webservern

using Authentication.Contexts;
using Authentication.Entities;
using Authentication.Handlers;
using Authentication.Interfaces;
using Authentication.Models;
using Authentication.Services;
using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Data.Seeders;
using Domain.Handlers;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using WebApi.Extensions.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// H�r kan man l�gga olika policies som h�mtas av UseCors().
// Under uteckling, AllowAll. Under utveckling, Strict.
// Se lektionsdemo 3.
builder.Services.AddCors();

builder.Services.AddDbContext<DataContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringData")));

//Konfiguerar auth-databasen mot connection-string
builder.Services.AddDbContext<AuthContext>(x =>
x.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringAuthentication")));

// Best�mmer vilken vilken context som ska anv�ndas med kommunikation med Identity
builder.Services.AddIdentity<AppUserEntity, IdentityRole>().AddEntityFrameworkStores<AuthContext>();

// Registrerar Interface och dess implementation i DI-containern (builder.Services)
// S�tts till scoped d� en ny instans ska skapas per HTTP-beg�ran
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
// Registrerar StatusService i DI-containern
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientContactInformationRepository, ClientContactInformationRepository>();
builder.Services.AddScoped<IClientAddressRepository, ClientAddressRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.Services.AddTransient<JwtTokenHandler>();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
// Standardiseat s�tt att skriva API-dokumentation p�.
builder.Services.AddOpenApi();

var azureConnString = builder.Configuration.GetConnectionString("AzureBlobStorage");
var containerName = "images";
builder.Services.AddScoped<IAzureFileHandler>(_ => new AzureFileHandler(azureConnString!, containerName));

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.ExampleFilters();

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v. 1.0",
        Title = "Alpha BackOffice API Documentation",
        Description = "Standard documentation for Alpha BackOffice portal"
    });

    var apiAdminScheme = new OpenApiSecurityScheme
    {
        Name = "X-ADM-API-KEY",
        Description = "Admin Api-Key Required",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme",
        Reference = new OpenApiReference
        {
            Id = "AdminApiKey",
            Type = ReferenceType.SecurityScheme,
        }
    };
    options.AddSecurityDefinition("AdminApiKey", apiAdminScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { apiAdminScheme, new List<string>() }
    });
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

builder.Services.AddAuthentication(x =>
{
    // Token-autentiering genom JwtBearerDefaults
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Verifierar att token-nyckeln �r giltlig
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!);

    var issuer = builder.Configuration["Jwt:Issuer"]!;

    // S�tts till true i produktionsmilj�
    x.RequireHttpsMetadata = false;
    // Token sparas i cache-minnet, i princip.
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.FromMinutes(5),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = false
    };

    x.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.NoResult();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\": \"Invalid token\"}");
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\": \"Authentication needed\"}");
        }
    };
});

var app = builder.Build();

// H�r instansieras rollerna.
await SeedData.SetRolesAsync(app);

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Alpha_backend_API");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

// F�r att validera att applikationen k�rs i en betrodd milj� med API-nyckel
//app.UseMiddleware<DefaultApiKeyMiddleware>();

// Vem anv�ndaren �r (claims osv)
app.UseAuthentication();
// Vad anv�ndaren f�r komma �t
app.UseAuthorization();

app.MapControllers();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();