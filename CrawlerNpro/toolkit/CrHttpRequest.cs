using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro
{
  public  class CrHttpRequest
    {
        public async  Task<string> SentDataAsync(HttpMethod httpMethod,string requesturl,HttpContent postContent=null,string cookies=null)
        {
            HttpClientHandler httpHandler = new HttpClientHandler();
            httpHandler.AllowAutoRedirect = true;
            httpHandler.UseCookies = false;
            httpHandler.AutomaticDecompression = System.Net.DecompressionMethods.GZip;
            using (HttpClient httpClient = new HttpClient(httpHandler))
            {
                httpClient.DefaultRequestHeaders.ExpectContinue = false;
                httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                httpClient.DefaultRequestHeaders.Add("Accept-Language", " zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; Touch; rv:11.0) like Gecko");

            //var response = await httpClient.GetAsync(sUrl);
            //return (await response.Content.ReadAsAsync<AvResult>());
            string strResult;
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(httpMethod, requesturl);
                bool isNullCookie = string.IsNullOrEmpty(cookies);
                if (!isNullCookie)
                {
                    //httpHandler.CookieContainer.SetCookies(new Uri(requestUrl), cookies);
                    request.Headers.Add("Cookie", cookies);
                }
                if (httpMethod.Method == "POST")
                    request.Content = postContent;
                var response = await httpClient.SendAsync(request);
                strResult = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
                return strResult;
            }
        }
    }
}
