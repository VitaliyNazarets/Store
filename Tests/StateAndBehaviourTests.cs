using Store.Adapters;
using Store.Data;
using Store.Enums;
using Store.Interfaces;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xunit;

namespace Tests
{
	public class StateAndBehavourTests
	{
		public readonly IUserSource userSource;
		public IDataSource<IProduct> dataSource;

		public AdminAdapter adminAdapter;
		public StateAndBehavourTests()
		{
			userSource = new UserSource();
			dataSource = new DataSource<IProduct>();
			adminAdapter = new AdminAdapter(userSource, dataSource);
		}

		[Theory]
		[InlineData(UserState.Closed)]
		[InlineData(UserState.NotVerified)]
		public async Task AdminAdapterThrowsException(UserState state)
		{
			await GenerateUser(state);
			var firstUser = await userSource.GetAsync(1);
			var Products = new List<IProduct>()
			{
				new Product()
				{
					Name = "Name 1",
					Price = 1
				},
				new Product()
				{
					Name = "Name 2",
					Price = 2
				}
			};

			var ex = await Assert.ThrowsAsync<Exception>(async () => await adminAdapter.UpdateProductsAsync(firstUser, Products));

			Assert.Equal("Forbidden", ex.Message);
		}

		[Fact]
		public async Task AdminAdapterSuccessPath()
		{
			await GenerateUser(UserState.Admin);
			var firstUser = await userSource.GetAsync(1);
			var Products = new List<IProduct>()
			{
				new Product()
				{
					Name = "Name 1",
					Price = 1
				},
				new Product()
				{
					Name = "Name 2",
					Price = 2
				}
			};
			
			await adminAdapter.UpdateProductsAsync(firstUser, Products);	

			Assert.Equal(Products.Count, dataSource.Count());
		}

		[Fact]
		public async Task CloseAccountBehavourTest()
		{
			await GenerateUser(UserState.Verified);

			await adminAdapter.CloseByEmailAsync("Test");

			var user = await userSource.FindAsync(email: "Test");

			Assert.Equal(UserState.Closed, user.UserState);
		}

		[Fact]
		public async Task UpgradeAccountAndAddBehavourTest()
		{
			var user = await GenerateUser(UserState.Verified);

			await adminAdapter.UpgradeAsync(user, UserState.Admin);

			var products = new List<IProduct>()
			{
				new Product()
				{
					Name = "Name 1",
					Price = 1
				},
				new Product()
				{
					Name = "Name 2",
					Price = 2
				}
			};

			await adminAdapter.UpdateProductsAsync(user, products);

			var verifyUser = await userSource.FindAsync(email: "Test");


			Assert.Equal(UserState.Admin, verifyUser.UserState);

			Assert.Equal(products.Count, dataSource.Count());

			var verifySource = await dataSource.GetIEnumerableAsync();


			foreach (var product in verifySource)
			{
				Assert.Contains(product, products);
			}
			foreach (var product in products)
			{
				Assert.Contains(product, verifySource);
			}
		}


		private async Task<IUser> GenerateUser(UserState state)
		{
			var user = new User()
			{
				Id = 1,
				Email = "Test",
				Phone = "0123",
				Name = "Name",
				UserState = state
			};
			await userSource.AddAsync(user);
			return user;
		}
	}
}
