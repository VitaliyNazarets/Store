using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Interfaces;

namespace Store.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ShopController : ControllerBase
	{
		private readonly IShopRepository<IProduct> _shopRepository;
		public ShopController(IShopRepository<IProduct> shopRepository)
		{
			_shopRepository = shopRepository;
		}

		[Route("Refresh")]
		public async Task<HttpResponseMessage> RefreshAsync()
		{
			try
			{
				await _shopRepository.UpdateNewProductsAsync();
				return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
			}
			catch (Exception e)
			{
				return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
				{
					Content = new StringContent(JsonConvert.SerializeObject(e.Message)) 
				};
			}
		}

		[Route("CalculatePrice")]
		public decimal Get(IEnumerable<IProduct> products) => _shopRepository.CalculateProductsPrice(products);

		[Route("GetProducts")]
		public async Task<IEnumerable<IProduct>> GetProductsAsync() => await _shopRepository.GetProductsAsync();

		[Route("GetProduct/{name}")]
		public async Task<IProduct> GetProductAsync(string name) => await _shopRepository.GetProductByNameAsync(name);
	}
}
