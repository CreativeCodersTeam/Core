using CreativeCoders.AspNetCore.Jwt;
using CreativeCoders.AspNetCore.TokenAuth;
using CreativeCoders.Core.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApiSampleApp;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.
            AddControllers()
            .AddApplicationPart(typeof(TokenAuthController).Assembly);

        services.AddJwtSupport<DefaultUserAuthProvider>(RandomString.Create());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        //app.UseMvc(x => {x.MapRoute())

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}

public class DefaultUserAuthProvider : IUserAuthProvider
{
    public bool CheckUser(string userName, string password, string domain)
    {
        return true;
    }
}