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
        public async Task TestLoginAsynch()
        {
            string USER_NAME = "5555555";
            string PASSWORD = "666666";

            var pw = await Playwright.CreateAsync();
            var browser = await pw.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            var resp = await page.GotoAsync("https://www.leumi.co.il/");

            var formButton = page.Locator("text=כניסה לחשבונך");
            await formButton.ClickAsync();
            Assert.IsTrue(page.Url.StartsWith("https://hb2.bankleumi.co.il/staticcontent/gate-keeper/"));

            var form = page.GetByRole(AriaRole.Form);
            var userName = page.GetByPlaceholder("שם משתמש");
            await userName.FillAsync(USER_NAME);
            string userNameText = await userName.InputValueAsync();
            Assert.AreEqual(userNameText, USER_NAME);

            var password = page.GetByPlaceholder("סיסמה");
            await password.FillAsync(PASSWORD);
            Assert.AreEqual(await password.InputValueAsync(), PASSWORD);

            var button = page.Locator("text=כניסה לחשבון");
            await button.ClickAsync(new()
            {
                Force = true
            });
            var warning = page.GetByText("אחד או יותר מפרטי ההזדהות שמסרת שגויים. ניתן לנסות שוב");

            bool isVisible =  warning.IsVisibleAsync().Result;

            Assert.IsTrue(isVisible);
            Assert.IsTrue(page.Url.Equals("https://hb2.bankleumi.co.il/eBanking/SO/SPA.aspx#/hpsummary"));
        }
    }
}