using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hack.Edge
{
    public class Startup
    {
	    private readonly IConfiguration _configuration;

	    public Startup(IConfiguration configuration) => _configuration = configuration;

	    public void ConfigureServices(IServiceCollection services)
	    {
		    services.AddProxy();
		    services.Configure<RawCaptureOptions>(_configuration);
	    }

        public void Configure(IApplicationBuilder app)
        {
	        app.UseWhen(context => context.Request.Path.StartsWithSegments("/ok"), builder => builder.UseMiddleware<OkMiddleware>());
	        app.UseMiddleware<RawCaptureMiddleware>();
			app.RunProxy(_configuration.GetSection("Proxy").GetProxyOptions());
        }
    }
}
