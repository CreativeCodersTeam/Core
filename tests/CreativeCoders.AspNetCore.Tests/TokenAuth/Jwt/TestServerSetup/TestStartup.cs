using System.Text;
using CreativeCoders.AspNetCore.TokenAuth.Jwt;
using CreativeCoders.AspNetCore.TokenAuthApi;
using CreativeCoders.AspNetCore.TokenAuthApi.Jwt;
using CreativeCoders.Core.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;

public class TestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddTokenAuthApiController()
            .AddApplicationPart(typeof(TestStartup).Assembly);

        services.AddAuthorization(x =>
            x.AddPolicy("TestPolicy",
                policy => policy.RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)));
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
