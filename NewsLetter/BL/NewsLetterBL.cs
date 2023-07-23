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
        public async Task ExecuteAsync()
        {
            List<ItemModel> news = new List<ItemModel>();

            var pw = await Playwright.CreateAsync();
            var browser = await pw.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            var resp = await page.GotoAsync("https://www.93fm.co.il/");

            IReadOnlyList<ILocator> headers = await page.Locator("id=timeline-infinite-scroll").GetByRole(AriaRole.Article).AllAsync();
            foreach (ILocator item in headers)
            {
                news.Add(new ItemModel()
                {
                    Date = await item.Locator("css=date").IsVisibleAsync() ? item.Locator("css=date").InnerHTMLAsync().Result : DateTime.Now.ToLongDateString(),
                    Author = await item.Locator("css=author").IsVisibleAsync() ? await item.Locator("css=author").InnerHTMLAsync() : "",
                    Excerpt = await item.GetByRole(AriaRole.Paragraph).IsVisibleAsync() ? await item.GetByRole(AriaRole.Paragraph).InnerHTMLAsync() : ""
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
