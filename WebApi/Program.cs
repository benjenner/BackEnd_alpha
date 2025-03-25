// Här skapas webservern

using Authentication.Contexts;
using Authentication.Interfaces;
using Authentication.Models;
using Authentication.Services;
using Business.Interfaces;
using Business.Managers;
using Business.Services;
using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Här kan man lägga olika policies som hämtas av UseCors().
// Under uteckling, AllowAll. Under utveckling, Strict.
// Se lektionsdemo 3.
builder.Services.AddCors();

builder.Services.AddDbContext<DataContext>(x =>
    x.UseLazyLoadingProxies()
    .UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringData")));

// Konfiguerar auth-databasen mot connection-string
builder.Services.AddDbContext<AuthContext>(x =>
x.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringAuthentication")));

// Bestämmer vilken vilken context som ska användas med kommunikation med Identity
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AuthContext>();

// Registrerar Interface och dess implementation i DI-containern (builder.Services)
// Sätts till scoped då en ny instans ska skapas per HTTP-begäran
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
// Registrerar StatusService i DI-containern
builder.Services.AddScoped<StatusService>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientContactInformationRepository, ClientContactInformationRepository>();
builder.Services.AddScoped<IClientAddressRepository, ClientAddressRepository>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<ITokenManager, TokenManager>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapOpenApi();
app.UseHttpsRedirection();

//app.UseSwaggerUI(x =>
//{
//    x.SwaggerEndpoint("/swagger/v1/swagger.json", "Alpha API");
//    x.RoutePrefix = string.Empty;
//});

// Identifierar om en endpoint har en autensiering (visar inte websidan om villkor inte uppfylls)
app.UseAuthorization();

app.UseAuthentication();
app.MapControllers();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();