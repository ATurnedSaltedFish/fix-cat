using CrawlerNpro.entity;
using CrawlerNpro.ServiceInput;
using CrawlerNpro.sqlDAL;
using CrawlerNpro.toolkit;
using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrawlerNpro.ViewControl
{
    public class CrawlerNpro
    {
        public async void SearchStart(string url)
        {
            Elements elements;
            string strGetConetex = null;
           // var url = this.txtUri.Text.Trim();
         
            CrHttpRequest crHttpRequest = new CrHttpRequest();
            strGetConetex = await crHttpRequest.SentDataAsync(HttpMethod.Post, url);
            if (strGetConetex.Length > 0)
            {
                Regex regUrl = new Regex(@"(/p/[0-9]{10})");
                Regex regTitle = new Regex(">(.*?)<");
                //this.txtContent.Text = strGetConetex;
                Document document = NSoup.NSoupClient.Parse(strGetConetex);
                Elements elementTbNum = document.GetElementsByClass("card_infoNum");
                Elements elementTbName = document.GetElementsByClass("card_title_fname");
                Elements elementTbTitle = document.Select("li.j_thread_list").Select("li.clearfix");
                //获取内容块
                //    Elements elementTbTitle =document.Select("div.threadlist_title").Select("div.pull_left ").Select("div.j_th_tit");//获取 发帖得标题 和标签得 url
                //     string data = "发帖数:" + elementTbNum.ToString() + "贴吧名称:" + elementTbName.ToString() + elementTbTitle;
                //his.txtContent.Text = data;

                //NSoup.Nodes.Document docomentChild = NSoup.NSoupClient.Parse(elementTbTitle.ToString());
                //Elements elementTbChildReplies = docomentChild.Select("span.threadlist_rep_num").Select("span.center_text");
                //Elements elementTbUrl = docomentChild.Select("a.j_th_tit");
                //this.txtContent.Text = elementTbTitle.ToString();
                List<TitleContentEntity> listContentEntity = new List<TitleContentEntity>();//内容
                List<ResultEntity> listResultEntity = new List<ResultEntity>();//所有内容
                try
                {
                    for (int i = 0; i < elementTbTitle.Count; i++)
                    {
                        var elementss = elementTbTitle[i];
                        //  Document docomentChild = NSoup.NSoupClient.Parse(elementTbTitle.ToString());
                        Elements elementTbChildReplies = elementss.Select("span.threadlist_rep_num").Select("span.center_text");
                        Elements elementTbUrl = elementss.Select("a.j_th_tit");
                        Debug.WriteLine(elementTbChildReplies + "/n/r" + elementTbUrl);

                        ResultEntity resultEntity = new ResultEntity();
                        if (elementTbUrl.ToString().Contains("href"))
                        {
                            // string reg = @"<a> *href=([""'])?(?(.*?)[^'""]+)\1[^>]*>";
                            //Regex regUrl = new Regex(@"(/p/[0-9]{10})");
                            //Regex regTitle = new Regex(">(.*?)<");
                            //<a href="/p/5388999580" title="求大佬帮忙p个图" target="_blank" class="j_th_tit ">求大佬帮忙p个图</a>
                            //Regex regUrl = new Regex(@"<a?* href=\/p\/^(.*?) */a> ");
                            var resultUrl = regUrl.Match(elementTbUrl.ToString()).ToString();

                            var tempData = regTitle.Match(elementTbUrl.ToString()).ToString();
                            var resultTitle = tempData.Substring(1, tempData.Length - 2);
                            resultEntity.Title = resultTitle;
                            resultEntity.Url = resultUrl;
                        }
                        if (elementTbChildReplies.ToString().Contains("回复"))
                        {
                            Regex regReplies = new Regex(@">(.*?)<");
                            var tempData = regReplies.Match(elementTbChildReplies.ToString()).ToString();
                            var resultReplies = tempData.Substring(1, tempData.Length - 2);
                            resultEntity.Replies = resultReplies;
                        }
                        listResultEntity.Add(resultEntity);
                    }
                    CompareHelper compareHelper = new CompareHelper();
                    List<ResultEntity> FilterListResultEntity = new List<ResultEntity>();//所有内容
                    var listkeyWorld = SelectKeyWorld();
                    foreach (var item in listResultEntity)
                    {
                        if (compareHelper.Compare(item.Title.Trim(), listkeyWorld, "KeyWorld"))
                        {
                            FilterListResultEntity.Add(item);
                        }
                    }
                    TBTitleListService tBTitleListService = new TBTitleListService();
                    tBTitleListService.InsertTBTItleList(MySqlConn.GetMysqlConn(), FilterListResultEntity);//添加到数据库
                }
                catch (Exception)
                {
                    throw;
                }

                //      //********************************************************************************//
                //    List<ContentEntity> listContentEntity = new List<ContentEntity>();//内容
                //    List<RepliesEneity> listRepliesEneity = new List<RepliesEneity>();//回复
                //    this.txtContent.Text = elementTbUrl.ToString();
                //    foreach (var itemPrplies in elementTbChildReplies)
                //    {
                //        RepliesEneity repliesEneity = new RepliesEneity();
                //        if (itemPrplies.ToString().Contains("回复"))
                //        {
                //            Regex regReplies = new Regex(@">(.*?)<");
                //            var tempData = regReplies.Match(itemPrplies.ToString()).ToString();
                //            var resultReplies = tempData.Substring(1, tempData.Length - 2);
                //            repliesEneity.Replies = resultReplies;
                //            listRepliesEneity.Add(repliesEneity);
                //        }
                //    }
                //    foreach (var itemUrl in elementTbUrl)
                //    {
                //        ContentEntity contentEntity = new ContentEntity();
                //        if (itemUrl.ToString().Contains("href"))
                //        {
                //            // string reg = @"<a> *href=([""'])?(?(.*?)[^'""]+)\1[^>]*>";
                //            Regex regUrl = new Regex(@"(/p/[0-9]{10})");
                //            //<a href="/p/5388999580" title="求大佬帮忙p个图" target="_blank" class="j_th_tit ">求大佬帮忙p个图</a>
                //            //Regex regUrl = new Regex(@"<a?* href=\/p\/^(.*?) */a> ");
                //            var resultUrl = regUrl.Match(itemUrl.ToString()).ToString();
                //            Regex regTitle = new Regex(">(.*?)<");
                //            var tempData = regTitle.Match(itemUrl.ToString()).ToString();
                //            var resultTitle = tempData.Substring(1, tempData.Length - 2);
                //            contentEntity.Title = resultTitle;
                //            contentEntity.Url = resultUrl;
                //            listContentEntity.Add(contentEntity);
                //        }
                //    }
                //    List<ResultEntity> listResultEntity = new List<ResultEntity>();//所有的
                //    //TBTitleListService tBTitleListService = new TBTitleListService();
                //    // tBTitleListService.InsertTBTItleList(MySqlConn.GetMysqlConn(), listTitleListEntity);添加到数据库
                //    //Elements elementTbChildTitle = docomentChild.Select("a.j_th_tit");
                //    //this.txtContent.Text = elementTbChildReplies.ToString()+"//**//"+"/n/r"+ elementTbChildTitle.ToString() +"//***//";
            }
        }

#if DEBUG
        public void InsertKeyWorld()
        {
            string txtFileUrl = @"C:\Users\LcAns\Desktop\SearchIndex.txt";
            KeyWorldService keyWorldService = new KeyWorldService();
            TBKeyWordDAL tBKeywordDAL = new TBKeyWordDAL();
            var list = keyWorldService.getTextContent(txtFileUrl);
            var result = tBKeywordDAL.InsertSysKeyworld(MySqlConn.GetMysqlConn(), list);
            if (result == 0)
            {
                Debug.WriteLine("insert index data failed");
            }
        }

        public List<KeyWorldEntity> SelectKeyWorld()
        {
            TBKeyWordDAL tBKeyWordDAL = new TBKeyWordDAL();
            var list = tBKeyWordDAL.SelectSysKeyworld(MySqlConn.GetMysqlConn(), null);
            if (list.Count < 0)
            {
                Debug.WriteLine("select index table failed");
                return null;
            }
            return list;
        }

#endif
    }
}
