using System.Text;
using CreativeCoders.AspNetCore.TokenAuth.Jwt;
using CreativeCoders.AspNetCore.TokenAuthApi;
using CreativeCoders.AspNetCore.TokenAuthApi.Jwt;
using CreativeCoders.Core.Text;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;

public class TestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddTokenAuthApiController();

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(RandomString.Create()));
        services.Configure<JwtTokenAuthApiOptions>(x => { x.SecurityKey = securityKey; });

        services.AddJwtTokenAuthentication(x => x.SecurityKey = securityKey);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
