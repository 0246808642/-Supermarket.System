using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Supermercado.CrossCutting.IoC;
using Supermercado.Application.Interfaces;
using Supermercado.Infrastructure.Services;
using Supermercado.Infrastructure.Data;
using Supermercado.Infrastructure.Identity;
using Supermercado.Api.Services;
using Supermercado.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OrderActionsBy(apiDesc => 
    {
        var controller = apiDesc.ActionDescriptor.RouteValues["controller"];
        // Faz o AuthController aparecer no topo
        return controller == "Auth" ? $"0_{controller}" : $"1_{controller}";
    });
});
builder.Services.AddControllers();

// Configure Database
builder.Services.AddDbContext<SupermercadoDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Identity
builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<SupermercadoDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// Jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();

// Current user service
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Register Application and Infrastructure services
builder.Services.RegisterServices();

builder.Services.AddHttpClient<IPaymentGatewayService, MercadoPagoService>(client => 
{
    var token = builder.Configuration["MercadoPago:AccessToken"];
    client.BaseAddress = new Uri("https://api.mercadopago.com/");
    if (!string.IsNullOrEmpty(token))
    {
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
});

var app = builder.Build();

// Execute Database Seeder
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SupermercadoDbContext>();
    await SupermercadoDbSeeder.SeedAsync(dbContext);

    try 
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        await SupermercadoDbSeeder.SeedIdentityAsync(userManager, roleManager);
    }
    catch (InvalidOperationException) 
    {
        // Identity not registered yet, will be configured in Fase 6.
        // It's safe to ignore for now if this runs before AddIdentityCore.
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
