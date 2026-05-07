using System.Text;
using BankApi.Application.Interfaces;
using BankApi.Application.UseCases.Accounts;
using BankApi.Application.UseCases.Auth;
using BankApi.Application.UseCases.Transactions;
using BankApi.Infrastructure.Auth;
using BankApi.Infrastructure.Context;
using BankApi.Infrastructure.Identity;
using BankApi.Infrastructure.Repositories;
using BankApi.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BankApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {

        services.AddDbContext<BankDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("default")));

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<BankDbContext>()
        .AddDefaultTokenProviders();

       
        var jwtSettings = config.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

        services.AddAuthentication(options =>
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
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });

  
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<Register>();
        services.AddScoped<Login>();
        services.AddScoped<CreateAccount>();
        services.AddScoped<GetAccountById>();
        services.AddScoped<GetMyAccounts>();
        services.AddScoped<Deposit>();
        services.AddScoped<Withdraw>();
        services.AddScoped<GetTransactionsByAccount>();

        return services;
    }
}
