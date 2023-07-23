// See https://aka.ms/new-console-template for more information
using ScrapBankAccount.BL;

ScrapBankAccountBL _scrapBankAccountBL = new ScrapBankAccountBL();
await _scrapBankAccountBL.LoginAsync();
await _scrapBankAccountBL.GetMovementsAsync();
