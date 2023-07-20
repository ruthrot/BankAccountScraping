using BankAccountScraping.Models;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountScraping
{
    public class ScrapBankAccountBL
    {
        public async Task Login(string userName, string password)
        {
            using var pw = await Playwright.CreateAsync();
            await using var browser = await pw.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();

            var resp = await page.GotoAsync("https://www.leumi.co.il/");

            page.GetByText("כניסה לחשבונך").ClickAsync().GetAwaiter().GetResult();

            //var resp = await page.GotoAsync("https://hb2.bankleumi.co.il/staticcontent/gate-keeper/he/?trackingCode=95510f1c-cf46-481f-7ec4-607dec15965f&sysNum=23&langNum=1&proxyName=PRD_KSV");

            //// Perform login.
            // page.GetByPlaceholder("שם משתמש").FillAsync(userName).GetAwaiter().GetResult();
            // page.GetByPlaceholder("סיסמה").FillAsync(password).GetAwaiter().GetResult();
            // page.GetByText("כניסה לחשבון").ClickAsync().GetAwaiter().GetResult();

        }

        public async Task<string> Execute(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<MovementModel> movements = new List<MovementModel>();
                string outputFile = $"{Environment.CurrentDirectory}/MovemntsFor_{fromDate.ToString("ddMMyyyy")}_{toDate.ToString("ddMMyyyy")}";
                using var pw = await Playwright.CreateAsync();
                await using var browser = await pw.Chromium.LaunchAsync();

                var page =  browser.NewPageAsync().Result;
                
                var resp =  page.GotoAsync("https://hb2.bankleumi.co.il/eBanking/SO/SPA.aspx#/ts/BusinessAccountTrx?Index=1").Result;

                ILocator title = page.GetByTitle("תנועות בחשבון");
                ILocator form = page.GetByRole(AriaRole.Form);
                ILocator table = form.GetByRole(AriaRole.Table);
                //ILocator locator = table.GetByRole(AriaRole.Row);
                bool isLogin = page.GetByText("ערב טוב").IsVisibleAsync().Result;
                ILocator locator = page.GetByRole(AriaRole.Row);

                var rows = locator.AllAsync().Result;

                foreach (var row in rows)
                {
                    var cells = row.GetByRole(AriaRole.Cell).AllAsync().Result;
                    movements.Add(new MovementModel
                    {
                        MovementDesc = cells[0].InnerTextAsync().Result
                    });
                }

                return "";
            }
            catch(Exception ex)
            {
                return ex.Message;

            }
            //<div _ngcontent-gfa-c144="" role="row" xlrow="" class="ts-table-row ng-star-inserted">
        }
    }
}
