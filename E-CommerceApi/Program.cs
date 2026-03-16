using System.Text;
using E_CommerceApi.Data;
using E_CommerceApi.Exceptions;
using E_CommerceApi.Interfaces;
using E_CommerceApi.Repository;
using Microsoft.EntityFrameworkCore;
using E_CommerceApi.Service;
using E_CommerceApi.Service.External;
using Microsoft.AspNetCore.Identity;
using E_CommerceApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = context.HttpContext.Request.Path;
    };
});

builder.Services.AddHttpClient<JokeApiClient>(client =>
    client.BaseAddress = new Uri("https://api.chucknorris.io/")
);

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

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
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
        };
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


#pragma warning disable EXTEXP0018 // HybridCache är preview/experimentellt beroende på specifik .NET 9-version
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

var app = builder.Build();

app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error; 

        var problemDetails = exception switch
        {
            NotFoundException ex => new ProblemDetails
            {
                Status = 404,
                Title = "Not Found",
                Detail = ex.Message
            },
            _ => new ProblemDetails
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = "Unexpected error occurred. Try again later."
            }
        };

        context.Response.StatusCode = problemDetails.Status ?? 500;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

async Task SeedAdminAsync(IServiceProvider services)
{
    var userManager = services.GetRequiredService<UserManager<User>>();
    var config = services.GetRequiredService<IConfiguration>();
    
    var adminEmail = config["AdminUser:Email"];
    var adminPassword = config["AdminUser:Password"];
    
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
        admin = new User
        {
            UserName = adminEmail, Email = adminEmail, FirstName = "System", LastName = "Admin"
        };
         
        var result = await userManager.CreateAsync(admin, adminPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
    else
    {
        var roles =  await userManager.GetRolesAsync(admin);
        if (!roles.Contains("Admin"))
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    await SeedAdminAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

