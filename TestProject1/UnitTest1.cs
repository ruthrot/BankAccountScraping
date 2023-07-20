using Microsoft.Playwright;
using System.Text.RegularExpressions;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestLogin()
        {
            string USER_NAME = "5555555";
            string PASSWORD = "666666";

            var pw = Playwright.CreateAsync().Result;
            var browser = await pw.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            var resp = await page.GotoAsync("https://www.leumi.co.il/");
            
            var formButton = page.Locator("text=כניסה לחשבונך");
             formButton.ClickAsync().GetAwaiter().GetResult();
            Assert.IsTrue(page.Url.StartsWith("https://hb2.bankleumi.co.il/staticcontent/gate-keeper/"));

            var form = page.GetByRole(AriaRole.Form);
            var userName = page.GetByPlaceholder("שם משתמש");
            userName.FillAsync(USER_NAME).GetAwaiter().GetResult();
            Assert.AreEqual(userName.InputValueAsync().Result, USER_NAME);

            var password = page.GetByPlaceholder("סיסמה");
            password.FillAsync(PASSWORD).GetAwaiter().GetResult();
            Assert.AreEqual(password.InputValueAsync().Result, PASSWORD);

            var button = form.Locator("BUTTON[type=\"submit\"]");
            button.ClickAsync(new() { Force = true });
            var warning = page.GetByText("אחד או יותר מפרטי ההזדהות שמסרת שגויים. ניתן לנסות שוב");
            Assert.IsTrue(warning.IsVisibleAsync().Result);
            Assert.IsTrue(page.Url.Equals("https://hb2.bankleumi.co.il/eBanking/SO/SPA.aspx#/hpsummary"));
        }
    }
}