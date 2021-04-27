using Moq;
using Store.Interfaces;
using Store.Repositories;
using Xunit;
using Store.Data;
using System.Threading.Tasks;
using Store.Models;
using System.Collections.Generic;

namespace Tests
{
	public class DataDrivenUnitTests
	{
		private readonly IDataSource<IProduct> source;

		private static readonly Product[] products = new Product[]
		{
			new Product()
			{
				Name = "Product 1",
				Price = 12
			},
			new Product()
			{
				Name = "Product 2",
				Price = 23
			}
		};

		public DataDrivenUnitTests()
		{
			source = new DataSource<IProduct>();
		}

		[Theory]
		[InlineData("1", 100.00, false)]
		[InlineData("12", -10, false)]
		[InlineData(null, 12, false)]
		[InlineData("Product name", 0, true)]
		[InlineData("Product name 2", 10000, true)]
		public async Task DataTestsAdd(string name, decimal price, bool isCorrect)
		{
			bool isCompletedSuccessfully = true;
			try
			{
				await source.AddAsync(new Product { Name = name, Price = price });
			}
			catch
			{
				isCompletedSuccessfully = false;
			}
			Assert.Equal(isCorrect, isCompletedSuccessfully);
		}

		[Theory]
		[InlineData("43" , false)]
		[InlineData("Product 1", true)]
		[InlineData("Product 2", true)]
		public async Task DataTestsGet(string name, bool isCorrect)
		{
			foreach (var product in products)
			{
				await source.AddAsync(product);
			}

			bool isCompletedSuccessfully = true;
			try
			{
				await source.GetAsync(name);
			}
			catch
			{
				isCompletedSuccessfully = false;
			}
			Assert.Equal(isCompletedSuccessfully, isCorrect);
		}
	}
}
