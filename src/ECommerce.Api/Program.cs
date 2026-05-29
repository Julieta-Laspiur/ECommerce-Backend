using System.Text;
using System.Security.Claims;
using ECommerce.Application.UseCases.Orders.Commands;
using ECommerce.Application.UseCases.Orders.Queries;
using ECommerce.Application.UseCases.Products.Commands;
using ECommerce.Application.UseCases.Products.Queries;
using ECommerce.Application.UseCases.Users.Commands;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ECommerce.Application.Interfaces.ITokenService, JwtTokenService>();

builder.Services.AddScoped<IGetAllProductsUseCase, GetAllProductsQuery>();

builder.Services.AddScoped<IGetProductByIdUseCase, GetProductByIdQuery>();

builder.Services.AddScoped<ICreateProductUseCase, CreateProductCommand>();

builder.Services.AddScoped<IDeleteProductUseCase, DeleteProductCommand>();

builder.Services.AddScoped<IRegisterUserUseCase, RegisterUserCommandHandler>();

builder.Services.AddScoped<ILoginUseCase, LoginCommandHandler>();

builder.Services.AddScoped<ICreateOrderUseCase, CreateOrderCommandHandler>();

builder.Services.AddScoped<IGetOrderByIdUseCase, GetOrderByIdQuery>();

builder.Services.AddScoped<IGetOrdersByUserUseCase, GetOrdersByUserQuery>();

var config = builder.Configuration;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config["Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = config["Jwt:Audience"],

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                RoleClaimType = ClaimTypes.Role,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            config["Jwt:Key"]!
                        ))
            };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "ECommerce API",
            Version = "v1"
        });

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Ingrese el token JWT"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});

builder.Services
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await DatabaseSeeder.SeedAsync(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
