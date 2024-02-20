using System.Security.Claims;
using System.Text;
using CreativeCoders.AspNetCore.TokenAuth.Jwt;
using CreativeCoders.AspNetCore.TokenAuthApi;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Jwt;
using CreativeCoders.Core.Text;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

namespace WebApiSampleApp;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers().AddTokenAuthApiController();

        builder.Services.AddScoped<IUserAuthProvider, DefaultUserAuthProvider>();
        builder.Services.AddScoped<IUserClaimsProvider, DefaultUserClaimsProvider>();

        builder.Services
            .AddJwtTokenAuthApi();

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(RandomString.Create()));
        builder.Services.Configure<JwtTokenAuthApiOptions>(x => { x.SecurityKey = securityKey; });

        builder.Services.AddJwtTokenAuthentication(x => x.SecurityKey = securityKey);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

[UsedImplicitly]
public class DefaultUserAuthProvider : IUserAuthProvider
{
    public Task<bool> AuthenticateAsync(string userName, string password, string? domain)
    {
        return Task.FromResult(true);
    }
}

[UsedImplicitly]
public class DefaultUserClaimsProvider : IUserClaimsProvider
{
    public Task<IEnumerable<Claim>> GetUserClaimsAsync(string userName, string? domain)
    {
        return Task.FromResult(new[]
        {
            new Claim(ClaimTypes.Name, userName)
        }.AsEnumerable());
    }
}
