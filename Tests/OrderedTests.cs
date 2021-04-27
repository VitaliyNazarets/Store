using Moq;
using Store.Data;
using Store.Interfaces;
using Store.Models;
using Store.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Orders;
using Xunit;

namespace Tests
{
	[TestCaseOrderer("Tests.Orders.PriorityOrderer", "Tests")]
    public class OrderedTests
    {
        public static bool AddDataCalled;
        public static bool CalculatePriceCalled;
        public static bool UpdatePricesCalled;
        public static bool PricesUpdatedCorrectlyCalled;
        public static Mock<IClient<Product>> client = new Mock<IClient<Product>>();
        public static IDataSource<Product> dataSource = new DataSource<Product>();
        public static IShopRepository<Product> shopRepository = new ShopRepository<Product>(client.Object, dataSource);

        [Fact, TestPriority(100)]
        public async Task PricesUpdatedCorrectlyTest()
        {
            PricesUpdatedCorrectlyCalled = true;

            var allProducts = await shopRepository.GetProductsAsync();

            var full_price = shopRepository.CalculateProductsPrice(allProducts);
            Assert.Equal(3811, full_price);

            Assert.True(AddDataCalled);
            Assert.True(CalculatePriceCalled);
            Assert.True(UpdatePricesCalled);
        }

        [Fact, TestPriority(1)]
        public async Task UpdateNewPricesTest()
        {
            UpdatePricesCalled = true;

           var task1 = shopRepository.GetProductByNameAsync("Product 1").ContinueWith(t => { 
            if (t.IsCompletedSuccessfully)
			   {
                   t.Result.Price = 111;
                   shopRepository.UpdateProductAsync(t.Result);
			   }
           });

            var task2 = shopRepository.GetProductByNameAsync("Product 2").ContinueWith(t => {
                if (t.IsCompletedSuccessfully)
                {
                    t.Result.Price = 200;
                    shopRepository.UpdateProductAsync(t.Result);
                }
            });

            var task3 = shopRepository.GetProductByNameAsync("Product 3").ContinueWith(t => {
                if (t.IsCompletedSuccessfully)
                {
                    t.Result.Price = 3000;
                    shopRepository.UpdateProductAsync(t.Result);
                }
            });

            var isUpdated = await Task.WhenAll(task1, task2, task3).ContinueWith(task => task.IsCompletedSuccessfully).ConfigureAwait(false);

            Assert.True(isUpdated);

            Assert.True(AddDataCalled);
            Assert.True(CalculatePriceCalled);
            Assert.False(PricesUpdatedCorrectlyCalled);
        }

        [Fact]
        public async Task CalculationPriceTest()
        {
            CalculatePriceCalled = true;

            var allProducts = await shopRepository.GetProductsAsync();

            var full_price = shopRepository.CalculateProductsPrice(allProducts);
            Assert.Equal(920, full_price);


            Assert.True(AddDataCalled);
            Assert.False(UpdatePricesCalled);
            Assert.False(PricesUpdatedCorrectlyCalled);
        }

        [Fact, TestPriority(-1)]
        public async Task AddDataTest()
        {
            AddDataCalled = true;

            List<Product> list = new List<Product>();
            for (int i = 0; i < 7; i++)
			{
                list.Add(new Product() { Name = $"Product {i}", Price = (i % 5) * 20 + 100 });
			}


            await shopRepository.AddProductsAsync(list).ConfigureAwait(false);

            Assert.Equal(7, shopRepository.Count());
            //verification of the order
            Assert.False(CalculatePriceCalled);
            Assert.False(UpdatePricesCalled);
            Assert.False(PricesUpdatedCorrectlyCalled);
        }
    }
}
