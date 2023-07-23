using Microsoft.Playwright;
using NewsLetter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace NewsLetter.BL
{
    public class NewsLetterBL
    {
        public async Task Execute()
        {
            List<ItemModel> news = new List<ItemModel>();

            var pw = Playwright.CreateAsync().Result;
            var browser = await pw.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            var resp = await page.GotoAsync("https://www.93fm.co.il/");

            IReadOnlyList<ILocator> headers = page.Locator("id=timeline-infinite-scroll").GetByRole(AriaRole.Article).AllAsync().Result;
            foreach (ILocator item in headers)
            {
                news.Add(new ItemModel()
                {
                    Date = item.Locator("css=date").IsVisibleAsync().Result ? item.Locator("css=date").InnerHTMLAsync().Result : DateTime.Now.ToLongDateString(),
                    Author = item.Locator("css=author").IsVisibleAsync().Result ? item.Locator("css=author").InnerHTMLAsync().Result : "",
                    Excerpt = item.GetByRole(AriaRole.Paragraph).IsVisibleAsync().Result ? item.GetByRole(AriaRole.Paragraph).InnerHTMLAsync().Result : ""
                });
            }

            string fileName = $"Newsletter_{DateTime.Now.ToString("ddMMyyyy HHmm")}.json";
            var options = new JsonSerializerOptions()
            {
                IncludeFields = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Hebrew),
            };
            var jsonString = JsonSerializer.Serialize(news, options);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
