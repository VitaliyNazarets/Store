using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Tests
{
	public class SeleniumWebTests
	{
		SeleniumAdapter seleniumAdapter;

		[Fact]
		public void YoutubePrimaryIconTest()
		{
			using (seleniumAdapter = new SeleniumAdapter("https://www.youtube.com"))
			{
				var driver = seleniumAdapter.GetDriver();
				driver.Manage().Window.Maximize();
				IWebElement youtubeIcon = driver.FindElement(By.Id("logo-icon"));
				youtubeIcon.Click();
			}
		}

		[Fact]
		public void YoutubeUrlSwitchingTest()
		{
			using (seleniumAdapter = new SeleniumAdapter("https://www.youtube.com/feed/trending"))
			{
				var driver = seleniumAdapter.GetDriver();
				driver.Manage().Window.Maximize();
				IWebElement youtubeIcon = driver.FindElement(By.Id("logo-icon"));
				youtubeIcon.Click();
				Assert.Equal("https://www.youtube.com/", driver.Url);
			}
		}

		[Fact]
		public void YoutubeContentTest()
		{
			using (seleniumAdapter = new SeleniumAdapter("https://www.youtube.com/feed/trending"))
			{
				var driver = seleniumAdapter.GetDriver();
				driver.Manage().Window.Maximize();
				IWebElement element = driver.FindElement(By.Id("grid-container"));
				var size = element.Size;
				Assert.True(size.Width > 100 && size.Height > 100);
			}
		}


	}

	internal class SeleniumAdapter : IDisposable
	{
		readonly IWebDriver driver;

		public SeleniumAdapter(string url = "https://www.youtube.com")
		{
			string path = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), @"ChromeDriver\");

			driver = new ChromeDriver(path)
			{
				Url = url
			};
		}

		public void Dispose()
		{
			driver.Close();
			driver.Dispose();
		}

		public IWebDriver GetDriver()
		{
			return driver;
		}
	}
}
