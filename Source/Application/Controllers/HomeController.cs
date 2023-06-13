using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Models.Configuration;
using Application.Models.Configuration.Extensions;
using Application.Models.Extensions;
using Application.Models.Net;
using Application.Models.Views;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Controllers
{
	public class HomeController : Controller
	{
		#region Constructors

		public HomeController(IConfiguration configuration, IDnsResolver dnsResolver, IOptionsMonitor<ForwardedHeadersOptions> forwardedHeadersOptionsMonitor, ILoggerFactory loggerFactory)
		{
			this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.DnsResolver = dnsResolver ?? throw new ArgumentNullException(nameof(dnsResolver));
			this.ForwardedHeadersOptionsMonitor = forwardedHeadersOptionsMonitor ?? throw new ArgumentNullException(nameof(forwardedHeadersOptionsMonitor));
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
		}

		#endregion

		#region Properties

		protected internal virtual IConfiguration Configuration { get; }
		protected internal virtual IDnsResolver DnsResolver { get; }
		protected internal virtual IOptionsMonitor<ForwardedHeadersOptions> ForwardedHeadersOptionsMonitor { get; }
		protected internal virtual ILogger Logger { get; }

		#endregion

		#region Methods

		protected internal virtual async Task<IDictionary<string, Exception>> GetIpAddresses()
		{
			var connection = this.HttpContext.Connection;
			var ipAddresses = new SortedDictionary<string, Exception>(StringComparer.OrdinalIgnoreCase);

			if(connection.LocalIpAddress != null)
				ipAddresses.Add(connection.LocalIpAddress.ToString(), null);

			if(connection.RemoteIpAddress != null && !ipAddresses.ContainsKey(connection.RemoteIpAddress.ToString()))
				ipAddresses.Add(connection.RemoteIpAddress.ToString(), null);

			foreach(var potentialIpAddress in await this.GetPotentialIpAddresses())
			{
				if(potentialIpAddress == null)
					continue;

				if(ipAddresses.ContainsKey(potentialIpAddress))
					continue;

				ipAddresses.Add(potentialIpAddress, IPAddress.TryParse(potentialIpAddress, out _) ? null : new InvalidOperationException("Invalid ip-address."));
			}

			return await Task.FromResult(ipAddresses);
		}

		protected internal virtual async Task<ISet<string>> GetPotentialIpAddresses()
		{
			var forwardedHeadersOptions = this.ForwardedHeadersOptionsMonitor.CurrentValue;
			var requestHeaders = this.Request.Headers;

			var potentialIpAddresses = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			foreach(var values in new[] { requestHeaders[forwardedHeadersOptions.ForwardedForHeaderName], requestHeaders[forwardedHeadersOptions.OriginalForHeaderName] })
			{
				foreach(var value in values.Where(value => value != null))
				{
					potentialIpAddresses.Add(value);
				}
			}

			const string forPrefix = "for=";

			foreach(var value in ((string)requestHeaders["forwarded"] ?? string.Empty).Split(';').Select(item => item.Trim()))
			{
				if(!value.StartsWith(forPrefix, StringComparison.OrdinalIgnoreCase))
					continue;

				foreach(var part in value[forPrefix.Length..].Split(',').Select(item => item.Trim()))
				{
					potentialIpAddresses.Add(part);
				}
			}

			return await Task.FromResult(potentialIpAddresses);
		}

		public virtual async Task<IActionResult> Index()
		{
			var model = new HomeViewModel
			{
				DefaultForwardedHeadersOptions = new ForwardedHeadersOptions().ToJson(),
				ForwardedHeadersEnabled = this.Configuration.IsEnabledSection(ConfigurationKeys.ForwardedHeadersPath),
				ForwardedHeadersOptions = this.ForwardedHeadersOptionsMonitor.CurrentValue.ToJson()
			};

			await this.PopulateIpToHostNameMap(model);

			return await Task.FromResult(this.View(model));
		}

		protected internal virtual async Task PopulateIpToHostNameMap(HomeViewModel model)
		{
			if(model == null)
				throw new ArgumentNullException(nameof(model));

			foreach(var (ipAddress, ipAddressException) in await this.GetIpAddresses())
			{
				if(ipAddressException != null)
				{
					model.IpToHostNameMap.Add(ipAddress, ipAddressException.Message);

					continue;
				}

				try
				{
					var hostName = this.DnsResolver.GetHostName(IPAddress.Parse(ipAddress));
					model.IpToHostNameMap.Add(ipAddress, hostName);
				}
				catch(Exception exception)
				{
					model.IpToHostNameMap.Add(ipAddress, exception.Message);
				}
			}
		}

		#endregion
	}
}