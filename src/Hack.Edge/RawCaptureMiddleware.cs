using System;
using System.Text;
using System.Threading.Tasks;
using Jil;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Hack.Edge
{
	internal sealed class RawCaptureMiddleware : IDisposable
	{
		private readonly RequestDelegate _next;
		private readonly EventHubClient _events;
		private readonly CloudBlobContainer _blob;

		public RawCaptureMiddleware(RequestDelegate next, IOptions<RawCaptureOptions> options)
		{
			_next = next;
			_events = EventHubClient.CreateFromConnectionString(options.Value.EventHub.ConnectionString);
			_blob = new CloudStorageAccount(
				new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
					options.Value.Blob.Name,
					options.Value.Blob.Key), true)
				.CreateCloudBlobClient()
				.GetContainerReference(options.Value.Blob.Container);
		}

		public Task Invoke(HttpContext context)
		{
			if (context.Request.Query.Count == 0 ||
			    context.Request.HasFormContentType ||
			    context.Request.Form.Files.Count != 1)
				return _next(context);

			return SplitRequest(context);
		}

		private async Task SplitRequest(HttpContext context)
		{
			await _blob.CreateIfNotExistsAsync();
			var blob = _blob.GetBlockBlobReference(context.TraceIdentifier);

			await Task.WhenAll(
				_events.SendAsync(new EventData(Encoding.UTF8.GetBytes(JSON.Serialize(context.Request.Query)))),
				blob.UploadFromStreamAsync(context.Request.Form.Files[0].OpenReadStream())
			);
		}

		public void Dispose() => _events?.Close();
	}
}