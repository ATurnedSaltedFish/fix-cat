using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;

namespace CrawlerNpro.toolkit
{
    public class UrlCodeConvert
    {

        public string EncodeUrl(string enCodeData)
        {
            return HttpUtility.UrlEncode(enCodeData, Encoding.UTF8);
        }

        public string DecodeUrl(string deCodeData)
        {
            return HttpUtility.UrlDecode(deCodeData, Encoding.UTF8);
        }
    }
}
