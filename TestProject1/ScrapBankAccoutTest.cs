using Microsoft.Playwright;
using ScrapBankAccount.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestLoginAsynch()
        {
            List<MovementModel> newMovementList = new List<MovementModel>();

            string USER_NAME = "";
            string PASSWORD = "";

            var pw = await Playwright.CreateAsync();
            var browser = await pw.Chromium.LaunchAsync(
            //    new BrowserTypeLaunchOptions
            //{
            //    Headless = false
            //}
                );

            var page = await browser.NewPageAsync();
            var resp = await page.GotoAsync("https://www.leumi.co.il/");

            var formButton = page.Locator("text=כניסה לחשבונך");
            await formButton.ClickAsync();
            
            Assert.IsTrue(page.Url.StartsWith("https://hb2.bankleumi.co.il/staticcontent/gate-keeper/"));

            await page.WaitForSelectorAsync("text=שם משתמש");
            Thread.Sleep(4000);
            var form = page.GetByRole(AriaRole.Form);
            var userNameInput = page.GetByPlaceholder("שם משתמש");
            await userNameInput.FillAsync(USER_NAME);
            string userNameText = await userNameInput.InputValueAsync();
            Assert.AreEqual(userNameText, USER_NAME);

            await page.WaitForSelectorAsync("text=סיסמה");
            var passwordInput = page.GetByPlaceholder("סיסמה");
            await passwordInput.FillAsync(PASSWORD);
            Assert.AreEqual(await passwordInput.InputValueAsync(), PASSWORD);

            page.Locator("text=כניסה לחשבון").ClickAsync(new()
            {
                Force = true
            });

            await page.WaitForSelectorAsync("text=העבר כסף");
          
            Assert.IsTrue(page.Url.Equals("https://hb2.bankleumi.co.il/eBanking/SO/SPA.aspx#/hpsummary"));

            var movementsButtun = page.GetByRole(AriaRole.Link).Locator("text=תנועות בחשבון");
            await movementsButtun.ClickAsync();

            await page.WaitForSelectorAsync("text=חיפוש מתקדם");
            var table = page.GetByRole(AriaRole.Table);
            var rows = await table.GetByRole(AriaRole.Row).AllAsync();
            foreach(var row in rows)
            {
                var cells = await row.GetByRole(AriaRole.Cell).AllAsync();
                if (cells.Count() > 5)
                {
                    string date = await cells[0].InnerTextAsync();
                    string desc = await cells[2].InnerTextAsync();
                    desc = Regex.Replace(desc, "(?=[א-ת]{1})[א-ת](?=[א-ת]{1})", "*");
                    string amount = await cells[4].InnerTextAsync(); 
                    if(String.IsNullOrEmpty(amount))
                        amount = await cells[5].InnerTextAsync();
                    newMovementList.Add(new MovementModel
                    {
                        Date = date,
                        Desc = desc,
                        Amount = amount,
                    });
                }
            }

            string fileName = $"Movements_{DateTime.Now.ToString("ddMMyyyy HHmm")}.json";
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