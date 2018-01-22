namespace Hack.Edge
{
	public class RawCaptureOptions
	{
		public EventHubOptions EventHub { get; set; }
		public BlobOptions Blob { get; set; }

		public class EventHubOptions
		{
			public string ConnectionString { get; set; }
		}

		public class BlobOptions
		{
			public string Name { get; set; }
			public string Key { get; set; }
			public string Container { get; set; }
		}
	}
}