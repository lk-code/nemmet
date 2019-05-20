using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace de.lkraemer.tagbuilder.tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            TagBuilder tbSmall = new TagBuilder("small");
            tbSmall.AddAttribute("class", "is--small-title");
            tbSmall.AddInnerHtml(" mit einem kleinen Text");

            string tag = "h1";
            TagBuilder tb = new TagBuilder(tag);
            tb.AddAttribute("id", "MainPageTitle");
            tb.AddAttribute("class", "is--link text-center");
            tb.AddInnerHtml("Das ist eine Überschrift");
            tb.AddInnerHtml(tbSmall.Render());

            string html = tb.Render();

            int i = 0;
        }
    }
}
