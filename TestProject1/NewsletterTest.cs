using Microsoft.Playwright;
using NewsLetter.BL;
using NewsLetter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace TestPlayWright
{
    public class NewsletterTest
    {

        [Test]
        public async Task TestNewsLetter()
        {
            NewsLetterBL newsLetterBL = new NewsLetterBL();
            newsLetterBL.Execute().GetAwaiter().GetResult();
        }
    }
  
}
