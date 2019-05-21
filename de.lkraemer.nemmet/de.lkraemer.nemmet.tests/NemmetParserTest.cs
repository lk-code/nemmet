using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace de.lkraemer.nemmet.tests
{
    [TestClass]
    public class NemmetParserTest
    {
        /// <summary>
        /// tests to generate a simple div with id and classes
        /// </summary>
        [TestMethod]
        public void TestGetHtmlWithSimpleDiv()
        {
            string tag = "div";
            string idValue = "AnotherId";
            string class1Value = "is--panel";
            string class2Value = "text-center";

            string nemmetCode = string.Format("{0}.{2}.{3}#{1}", tag, idValue, class1Value, class2Value);
            string html = NemmetParser.GetHtml(nemmetCode);

            Assert.IsTrue(html.Contains("<" + tag)); // the element must contains the start tag
            Assert.IsTrue(html.Contains("</" + tag + ">")); // the element must contains the end tag
            Assert.IsTrue(html.Contains("id=\"" + idValue + "\"")); // the element must contain an id
            Assert.IsTrue(html.Contains("class=\"" + class1Value + " " + class2Value + "\"")); // the element must contain a class
        }

        /// <summary>
        /// tests to generate a simple a (link) with classes and without an id
        /// </summary>
        [TestMethod]
        public void TestGetHtmlWithSimpleLinkWithoutId()
        {
            string tag = "a";
            string class1Value = "is--link";
            string class2Value = "text-center";

            string nemmetCode = string.Format("{0}.{1}.{2}", tag, class1Value, class2Value);
            string html = NemmetParser.GetHtml(nemmetCode);

            Assert.IsTrue(html.Contains("<" + tag)); // the element must contains the start tag
            Assert.IsTrue(html.Contains("</" + tag + ">")); // the element must contains the end tag
            Assert.IsFalse(html.Contains("id=\"")); // the element must not contain an id
            Assert.IsTrue(html.Contains("class=\"" + class1Value + " " + class2Value + "\"")); // the element must contain a class
        }
    }
}
