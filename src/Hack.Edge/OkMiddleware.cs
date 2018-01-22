using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Hack.Edge
{
	internal sealed class OkMiddleware
	{
		public OkMiddleware(RequestDelegate _) { }

		public Task Invoke(HttpContext context) => Task.CompletedTask;
	}
}