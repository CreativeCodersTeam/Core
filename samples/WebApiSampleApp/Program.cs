using System.Security.Claims;
using CreativeCoders.AspNetCore.TokenAuth;
using CreativeCoders.AspNetCore.TokenAuth.Abstractions;
using CreativeCoders.AspNetCore.TokenAuth.Jwt;
using CreativeCoders.Core.Text;
using JetBrains.Annotations;

namespace WebApiSampleApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers().AddTokenAuthApiController();

        builder.Services.AddJwtTokenAuthApi(x =>
        {
            //x.UseCookies = true;
        });
        builder.Services.AddJwtSupport<DefaultUserAuthProvider, DefaultUserClaimsProvider>(RandomString.Create());
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
    public Task<bool> AuthenticateAsync(string userName, string password, string domain)
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
