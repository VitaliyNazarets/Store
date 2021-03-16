using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

using System.IO;

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

		[Fact]
		public void FirstVideoFromTrendingTest()
		{
			using (seleniumAdapter = new SeleniumAdapter("https://www.youtube.com/feed/trending"))
			{
				var driver = seleniumAdapter.GetDriver();
				driver.Manage().Window.Maximize();
				var videos = driver.FindElements(By.ClassName("ytd-video-renderer"));
				Assert.True(videos.Count > 0);
				var video = videos[0];
				video.Click();
				Assert.StartsWith("https://www.youtube.com/watch", driver.Url);
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
