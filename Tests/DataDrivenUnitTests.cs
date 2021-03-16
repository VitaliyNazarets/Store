using Moq;
using Store.Interfaces;
using Store.Repositories;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	public class DataDrivenUnitTests
	{
		private readonly Mock<IClient<IProduct>> mockClient;
		private readonly Mock<IDataSource<IProduct>> mockSource;
		private readonly IShopRepository<IProduct> shopRepository;
		public DataDrivenUnitTests()
		{
			mockClient = new Mock<IClient<IProduct>>();
			mockSource = new Mock<IDataSource<IProduct>>();
			shopRepository = new ShopRepository<IProduct>(mockClient.Object, mockSource.Object);
		}

		[DataTestMethod]
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
			@"|DataDirectory|\TestAddData.csv",
			"TestMultiplication#csv",
			DataAccessMethod.Sequential)]
		public void DataTests()
		{

		}

	}
}
