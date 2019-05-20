using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace de.lkraemer.nemmet.tests
{
    [TestClass]
    public class NemmetTagTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string code = "#my-panel.panel>.heading{Title}+.content{Content}+.footer";

            List<NemmetTag> tags = NemmetTag.Parse(code);

            string html = NemmetParser.GetHtml(code);
        }
    }
}
