// See https://aka.ms/new-console-template for more information
using BankAccountScraping;

Microsoft.Playwright.Program.Main(new string[] { "install" });
DateTime fromDate = DateTime.Now.AddMonths(-1);
DateTime toDate = DateTime.Now;

ScrapBankAccountBL _scrapBankAccountBL = new ScrapBankAccountBL();

//_scrapBankAccountBL.Login("555555", "666666").GetAwaiter().GetResult();

var res =  _scrapBankAccountBL.Execute(fromDate, toDate).GetAwaiter().GetResult();


