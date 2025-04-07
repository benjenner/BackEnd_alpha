// Här skapas webservern

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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Extensions.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Här kan man lägga olika policies som hämtas av UseCors().
// Under uteckling, AllowAll. Under utveckling, Strict.
// Se lektionsdemo 3.
builder.Services.AddCors();

builder.Services.AddDbContext<DataContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringData")));

//Konfiguerar auth-databasen mot connection-string
builder.Services.AddDbContext<AuthContext>(x =>
x.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringAuthentication")));

// Bestämmer vilken vilken context som ska användas med kommunikation med Identity
builder.Services.AddIdentity<AppUserEntity, IdentityRole>().AddEntityFrameworkStores<AuthContext>();

// Registrerar Interface och dess implementation i DI-containern (builder.Services)
// Sätts till scoped då en ny instans ska skapas per HTTP-begäran
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
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(x =>
{
    // Token-autentiering genom JwtBearerDefaults
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Verifierar att token-nyckeln är giltlig
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!);

    var issuer = builder.Configuration["Jwt:Issuer"]!;

    // Sätts till true i produktionsmiljö
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
});

var app = builder.Build();

// Här instansieras rollerna.
await SeedData.SetRolesAsync(app);

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Alpha API"));
app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));
app.UseHttpsRedirection();
app.UseMiddleware<DefaultApiKeyMiddleware>();

// Vad användaren får komma åt
app.UseAuthorization();
// Vem användaren är (claims osv)
app.UseAuthentication();

app.MapControllers();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();