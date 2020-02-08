using System.Text;
using CreativeCoders.AspNetCore.TokenAuth;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.Jwt
{
    [PublicAPI]
    public static class JwtServicesRegistrationExtensions
    {
        public static IServiceCollection AddJwtSupport<TUserAuthProvider>(this IServiceCollection serviceCollection, string symSecurityKey)
            where TUserAuthProvider : class, IUserAuthProvider
        {
            serviceCollection
                .AddScoped<IUserAuthProvider, TUserAuthProvider>()
                .AddSingleton<ISymSecurityKeyConfig>(sp => new SymSecurityKeyConfig(symSecurityKey))
                .AddSingleton<ITokenHandler, JwtTokenHandler>()
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(symSecurityKey)),
                        ValidIssuer = string.Empty,
                        ValidAudience = string.Empty,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return serviceCollection;
        }

        public static IMvcBuilder AddJwtSupport<TUserAuthProvider>(this IMvcBuilder builder, string symSecurityKey)
            where TUserAuthProvider : class, IUserAuthProvider
        {
            builder.Services
                .AddJwtSupport<TUserAuthProvider>(symSecurityKey);

            return builder;
        }
    }
}