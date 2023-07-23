using Microsoft.Playwright;
using ScrapBankAccount.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ScrapBankAccount.BL
{
    public class ScrapBankAccountBL
    {
        IPage _page;
        public ScrapBankAccountBL()
        {
            var pw =  Playwright.CreateAsync().Result;
            var browser = pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            }).Result;

            IPage page =  browser.NewPageAsync().Result;
        }

        public async Task LoginAsync()
        {
            string USER_NAME = "";
            string PASSWORD = "";

            var pw = await Playwright.CreateAsync();
            var browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            var page = await browser.NewPageAsync();
            var resp = await page.GotoAsync("https://www.leumi.co.il/");

            var formButton = page.Locator("text=כניסה לחשבונך");
            await formButton.ClickAsync();
            await page.WaitForSelectorAsync("text=שם משתמש");
            var form = page.GetByRole(AriaRole.Form);
            var userNameInput = page.GetByPlaceholder("שם משתמש");
            await userNameInput.FillAsync(USER_NAME);
            string userNameText = await userNameInput.InputValueAsync();

            await page.WaitForSelectorAsync("text=סיסמה");
            var passwordInput = page.GetByPlaceholder("סיסמה");
            await passwordInput.FillAsync(PASSWORD);

            page.Locator("text=כניסה לחשבון").ClickAsync();
        }

        public async Task GetMovementsAsync()
        {
            List<MovementModel> newMovementList = new List<MovementModel>();

            await _page.WaitForSelectorAsync("text=העבר כסף");
           
            var movementsButtun = _page.GetByRole(AriaRole.Link).Locator("text=תנועות בחשבון");
            await movementsButtun.ClickAsync();

            await _page.WaitForSelectorAsync("text=חיפוש מתקדם");
            var table = _page.GetByRole(AriaRole.Table);
            var rows = await table.GetByRole(AriaRole.Row).AllAsync();
            foreach (var row in rows)
            {
                var cells = await row.GetByRole(AriaRole.Cell).AllAsync();
                if (cells.Count() > 5)
                {
                    string date = await cells[0].InnerTextAsync();
                    string desc = await cells[2].InnerTextAsync();
                    desc = Regex.Replace(desc, "(?=[ת-א]{1})[ת-א](?=[ת-א]{1})", "*");

                    string amount = await cells[4].InnerTextAsync();
                    if (String.IsNullOrEmpty(amount))
                        amount = await cells[5].InnerTextAsync();

                    newMovementList.Add(new MovementModel
                    {
                        Date = date,
                        Desc = desc,
                        Amount = amount,
                    });
                }
            }

            string fileName = $"Newsletter_{DateTime.Now.ToString("ddMMyyyy HHmm")}.json";
            var options = new JsonSerializerOptions()
            {
                IncludeFields = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Hebrew),
            };
            var jsonString = JsonSerializer.Serialize(newMovementList, options);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
