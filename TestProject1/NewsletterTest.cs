using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestPlayWright
{
    public class NewsletterTest
    {

        [Test]
        public async Task TestNewsLetter()
        {
            List<string> news = new List<string>();

            var pw = Playwright.CreateAsync().Result;
            var browser = await pw.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            var resp = await page.GotoAsync("https://www.93fm.co.il/");

            IReadOnlyList<ILocator> headers = page.GetByRole(AriaRole.Heading).GetByRole(AriaRole.Link).AllAsync().Result;
            foreach (ILocator item in headers)
            {
                news.Add(item.InnerTextAsync().Result);
            }

            string fileName = $"Newsletter_{DateTime.Now.ToString("ddMMyyyy HH:mm")}";
            var jsonString = JsonSerializer.Serialize(news);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
