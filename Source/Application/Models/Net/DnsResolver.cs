using System.Net;

namespace Application.Models.Net
{
	public class DnsResolver : IDnsResolver
	{
		#region Methods

		public virtual string GetHostName(IPAddress ipAddress)
		{
			return Dns.GetHostEntry(ipAddress).HostName;
		}

		#endregion
	}
}