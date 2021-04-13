using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Interfaces;
using Store.Models;

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
		[HttpGet]
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
		[Route("AddProducts")]
		[HttpPost]
		public async Task<HttpResponseMessage> AddProductAsync([FromBody] IEnumerable<Product> products)
		{
			try
			{
				var isFullyAdded = await _shopRepository.AddProductsAsync(products);
				return isFullyAdded ?
				 new HttpResponseMessage(System.Net.HttpStatusCode.OK)
				 {
					 Content = new StringContent(JsonConvert.SerializeObject("products added"))
				 }
				 :
				 new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
				 {
					 Content = new StringContent(JsonConvert.SerializeObject("products not fully added. Some of them is already exists!"))
				 };
			}
			catch (Exception e)
			{
				return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
				{
					Content = new StringContent(JsonConvert.SerializeObject(e.Message))
				};
			}
		}

		[Route("UpdateProduct")]
		[HttpPost]
		public async Task<HttpResponseMessage> UpdateProduct([FromBody] Product product)
		{
			try
			{
				await _shopRepository.UpdateProductAsync(product);
				return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
				 {
					 Content = new StringContent(JsonConvert.SerializeObject("product updated"))
				 };

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
		[HttpGet]
		public decimal GetPrice([FromBody] IEnumerable<Product> products) => _shopRepository.CalculateProductsPrice(products);

		[Route("GetProducts")]
		[HttpGet]
		public async Task<IEnumerable<IProduct>> GetProductsAsync() => await _shopRepository.GetProductsAsync();

		[Route("GetProduct/{name}")]
		[HttpGet]
		public async Task<IProduct> GetProductAsync(string name) => await _shopRepository.GetProductByNameAsync(name);
	}
}
