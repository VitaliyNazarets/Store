using Moq;
using Store.Interfaces;
using Store.Repositories;
using System;
using System.Collections.Generic;
using Xunit;
using Store.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Tests
{
	public class UnitTestMocks
	{
		private readonly Mock<IClient<IProduct>> mockClient;
		private readonly Mock<IDataSource<IProduct>> mockSource;
		private readonly IShopRepository<IProduct> shopRepository;
		public UnitTestMocks()
		{
			mockClient = new Mock<IClient<IProduct>>();
			mockSource = new Mock<IDataSource<IProduct>>();
			shopRepository = new ShopRepository<IProduct>(mockClient.Object, mockSource.Object);
		}
		private IProduct GenerateProduct(string Name, decimal Price) => new Product() { Name = Name, Price = Price };

		[Fact]
		public async Task UpdateProductsSuccess()
		{
			List<IProduct> list = new List<IProduct>();
			for (int i = 0; i < 5; i++)
				list.Add(GenerateProduct($"name {i}", i));
			mockClient.Setup(f => f.GetNewProductsAsync()).ReturnsAsync(list);
			mockSource.Setup(f => f.AddAsync(It.IsAny<IProduct>()));
			var exception = await  Record.ExceptionAsync(async () => await shopRepository.UpdateNewProductsAsync());
			Assert.Null(exception);
		}

		[Fact]
		public void UpdateProductsClientThrowsException()
		{
			mockClient.Setup(f => f.GetNewProductsAsync()).ThrowsAsync(new IndexOutOfRangeException());
			mockSource.Setup(f => f.AddAsync(It.IsAny<IProduct>()));
			Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await shopRepository.UpdateNewProductsAsync());
		}

		[Fact]
		public void UpdateProductsSourceThrowsException()
		{
			List<IProduct> list = new List<IProduct>();
			for (int i = 0; i < 5; i++)
				list.Add(GenerateProduct($"name {i}", i));
			mockClient.Setup(f => f.GetNewProductsAsync()).ReturnsAsync(list);
			mockSource.Setup(f => f.AddAsync(It.IsAny<IProduct>())).ThrowsAsync(new ArgumentException());
			Assert.ThrowsAsync<ArgumentException>(async () => await shopRepository.UpdateNewProductsAsync());
		}

		[Fact]
		public async Task GetProductByNameAsyncSuccess()
		{
			var expectedProduct = GenerateProduct("Name 1", 123);
			mockSource.Setup(f => f.GetAsync(It.IsAny<string>())).ReturnsAsync(expectedProduct);
			var result = await shopRepository.GetProductByNameAsync("Name 1");
			Assert.Equal(expectedProduct.Name, result.Name);
			Assert.Equal(expectedProduct.Price, result.Price);
		}

		[Fact]
		public void GetProductByNameAsyncThrowsException()
		{
			mockSource.Setup(f => f.GetAsync(It.IsAny<string>())).ThrowsAsync(new Exception());
			Assert.ThrowsAsync<Exception>(async () => await shopRepository.GetProductByNameAsync("Name 1"));
		}

		[Fact]
		public async Task GetProductsAsyncSuccess()
		{
			List<IProduct> expectedList = new List<IProduct>();
			for (int i = 0; i < 5; i++)
				expectedList.Add(GenerateProduct($"name {i}", i));
			mockSource.Setup(f => f.GetListAsync()).ReturnsAsync(expectedList);
			var result = await shopRepository.GetProductsAsync();
			Assert.Equal(expectedList.Count, result.Count());
			for (int i = 0; i < expectedList.Count; i++)
			{ 
				Assert.Equal(expectedList.ElementAt(i).Name, result.ElementAt(i).Name);
				Assert.Equal(expectedList.ElementAt(i).Price, result.ElementAt(i).Price);
			}
		}

		[Fact]
		public void GetProductsAsyncThrowsException()
		{
			mockSource.Setup(f => f.GetListAsync()).ThrowsAsync(new Exception());
			Assert.ThrowsAsync<Exception>(async () => await shopRepository.GetProductsAsync());
		}

	}
}
