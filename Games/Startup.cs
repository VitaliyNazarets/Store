using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Store.Clients;
using Store.Configs;
using Store.Data;
using Store.Interfaces;
using Store.Repositories;

namespace Store
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
		{
			return HttpPolicyExtensions
				.HandleTransientHttpError()
				.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
				.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddOptions();

			services.Add(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
			//var config = new ConfigurationBuilder()
			//   .AddJsonFile("appsettings.json", optional: false)
			//   .Build();

			services.Configure<WebDataOptions>(Configuration.GetSection("WebClient"));

			services.AddSingleton<IDataSource<IProduct>, DataSource<IProduct>>();
			services.AddHttpClient("HttpClient")
				.AddHttpMessageHandler(services => new LoggingHttpMessageHandler(services.GetRequiredService<ILoggerFactory>().CreateLogger("logger")))
				.AddPolicyHandler(GetRetryPolicy());

			services.AddSingleton<IClient<IProduct>, WebDataClient<IProduct>>(provider =>
			  new WebDataClient<IProduct>(provider.GetRequiredService<IHttpClientFactory>(),
				provider.GetService<IOptions<WebDataOptions>>(),
				provider.GetRequiredService<ILoggerFactory>().CreateLogger("WebLogger"))
			);

			services.AddScoped<IShopRepository<IProduct>>(provider=> 
			new ShopRepository<IProduct>(provider.GetRequiredService<IClient<IProduct>>(), provider.GetRequiredService<IDataSource<IProduct>>()));

			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
