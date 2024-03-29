using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Nmt.Core.Auth;
using Nmt.Core.Behaviors;
using Nmt.Core.BusEventConsumers;
using Nmt.Core.TokenProviders;
using Nmt.Domain.Common;
using Nmt.Domain.Configs;
using Nmt.Domain.Consts;

namespace Nmt.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection(nameof(JwtConfig)).Get<JwtConfig>()!;
        services.AddSingleton(jwtConfig);

        services.AddAuthentication(opts => 
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opts => 
        {
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidIssuer = jwtConfig.ValidIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
            };
        });

        services.AddAuthorization(opts =>
        {
            opts.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireClaim(AuthClaims.UserId);
            });
        });

        services.AddSingleton<IAuthorizationHandler, PermissionsAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionsAuthorizationPolicyProvider>();

        return services;
    }

    public static IServiceCollection AddRedisCache(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(ICachePolicy<,>)), false)
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(IService)), false)
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.AddTransient<TwoFactorCodeProvider>();

        return services;
    }

    public static IServiceCollection AddSmtpConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        var googleSmtpConfig = configuration.GetSection(nameof(GoogleSmtpConfig)).Get<GoogleSmtpConfig>()!;
        services.AddSingleton(googleSmtpConfig);

        return services;
    }
    
    public static IServiceCollection AddRabbitMQ(this IServiceCollection serviceCollection, IConfiguration configuration, bool includeConsumers = true)
    {
        var rabbitMqConnectionString = configuration.GetConnectionString("RabbitMQ");

        serviceCollection.AddMassTransit(config => 
        {
            if (includeConsumers)
            {
                config.AddConsumer<BlockIpAddressesEventConsumer>();
            }

            config.UsingRabbitMq((ctx, cfg) => 
            {
                cfg.Host(rabbitMqConnectionString);

                if (includeConsumers)
                {
                    cfg.ReceiveEndpoint(EventBusQueues.BlockIpAddressesQueue, endpoint =>
                    {
                        endpoint.ConfigureConsumer<BlockIpAddressesEventConsumer>(ctx);
                    });
                }
            });
        });

        return serviceCollection;
    }
}