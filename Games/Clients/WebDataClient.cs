using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Store.Configs;
using Store.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Store.Clients
{
	public class WebDataClient<IProduct> : IClient<IProduct>
	{
		private readonly  HttpClient _client;

		private readonly ILogger _logger;

		private readonly WebDataOptions _options;

		public WebDataClient(IHttpClientFactory httpFactory, IOptions<WebDataOptions> options, ILogger logger)
		{
			_client = httpFactory.CreateClient();
			_client.BaseAddress = new Uri(_options.DataUrl);
			_logger = logger;
			_options = options.Value;
		}
		public async Task<IEnumerable<IProduct>> GetNewProductsAsync()
		{
			_logger.LogInformation("Start getting new products");
			var task = await GetProductsAsync().ConfigureAwait(false);
			return JsonConvert.DeserializeObject<IEnumerable<IProduct>>(await task.Content.ReadAsStringAsync());
		}

		private async Task<HttpResponseMessage> GetProductsAsync()
		{
			return await _client.GetAsync($"{_options.DataUrl}").ConfigureAwait(false);
		}
	}
}
