using System.ComponentModel;
using System.Net;
using Application.Models.ComponentModel;
using Application.Models.Configuration;
using Application.Models.Configuration.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

TypeDescriptor.AddAttributes(typeof(IPAddress), new TypeConverterAttribute(typeof(IpAddressTypeConverter)));
TypeDescriptor.AddAttributes(typeof(IPNetwork), new TypeConverterAttribute(typeof(IpNetworkTypeConverter)));

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
	options.AllowedHosts.Clear();
	options.KnownNetworks.Clear();
	options.KnownProxies.Clear();
});

var forwardedHeadersSection = builder.Configuration.GetSection(ConfigurationKeys.ForwardedHeadersPath);

builder.Services.Configure<ForwardedHeadersOptions>(forwardedHeadersSection);

builder.Services.AddControllersWithViews();

var application = builder.Build();

application.UseDeveloperExceptionPage();

var configuration = application.Services.GetRequiredService<IConfiguration>();

if(configuration.IsEnabledSection(ConfigurationKeys.ForwardedHeadersPath))
	application.UseForwardedHeaders();

application.UseStaticFiles()
	.UseRouting()
	.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });

application.Run();