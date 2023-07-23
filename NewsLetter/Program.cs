// See https://aka.ms/new-console-template for more information
using NewsLetter.BL;

NewsLetterBL newsLetterBL = new NewsLetterBL();

newsLetterBL.Execute().GetAwaiter().GetResult();