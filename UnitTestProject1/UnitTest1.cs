using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            await Page.GotoAsync("https://www.leumi.co.il/");
            var formButton = Page.Locator("text=Open Contact Form");
            await formButton.ClickAsync();
            await Expect(Page).ToHaveURLAsync(new Regex(".*Home/Form"));
        }
    }
}
    }
}
