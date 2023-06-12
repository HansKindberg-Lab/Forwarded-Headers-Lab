using System;
using System.Threading.Tasks;
using Application.Models.Extensions;
using Application.Models.Views;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Controllers
{
	public class HomeController : Controller
	{
		#region Constructors

		public HomeController(ILoggerFactory loggerFactory)
		{
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
		}

		#endregion

		#region Properties

		protected internal virtual ILogger Logger { get; }

		#endregion

		#region Methods

		public virtual async Task<IActionResult> Index()
		{
			var forwardedHeadersOptions = this.HttpContext.RequestServices.GetRequiredService<IOptions<ForwardedHeadersOptions>>().Value;

			var model = new HomeViewModel
			{
				DefaultForwardedHeadersOptions = new ForwardedHeadersOptions().ToJson(),
				ForwardedHeadersOptions = forwardedHeadersOptions.ToJson()
			};

			return await Task.FromResult(this.View(model));
		}

		#endregion
	}
}