using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WatiN.MsHtmlBrowser;

namespace IA.Lecturas
{
    public class UrlUploader
    {
        public string ParserUrl(string url)
        {
            var browser = new MsHtmlBrowser();
            browser.GoTo(url);
            string contents = browser.Text;
            return contents;
        }
    }
}