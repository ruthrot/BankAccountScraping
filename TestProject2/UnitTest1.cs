using PuppeteerSharp;

namespace TestProject2
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1Async()
        {
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://www.leumi.co.il/");

            page.ClickAsync("input[type='button']").Wait();
            Assert.IsTrue(page.Url.StartsWith("https://hb2.bankleumi.co.il/staticcontent/gate-keeper/"));

            var userName = await page.QuerySelectorAsync("input[type='input']");
            userName.TypeAsync("7x8vq3y").Wait();
            Assert.AreEqual(userName.ToString(), "7x8vq3y");

            //var password = page.GetByPlaceholder("סיסמה");
            //password.TypeAsync("MR6423860").GetAwaiter().GetResult();
            //Assert.AreEqual(userName.TextContentAsync().Result, "MR6423860");

            //var button = page.GetByText("כניסה לחשבון");
            //button.ClickAsync().GetAwaiter().GetResult();
            //Assert.IsTrue(page.Url.Equals("https://hb2.bankleumi.co.il/eBanking/SO/SPA.aspx#/hpsummary"));

        }
    }
}