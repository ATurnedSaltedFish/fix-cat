using CrawlerNpro.entity;
using CrawlerNpro.Model;
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
using System.Configuration;
using CrawlerNpro.SqlDAL;

namespace CrawlerNpro.ViewControl
{
    public class CrawlerNproView
    {

        /// <summary>
        /// 搜索开始
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        public async void SearchStart(string url, string fileName)
        {
            string JsonContentPath = ConfigurationManager.AppSettings["JsonContentPath"];
            Elements elements;
            string strGetConetex = null;
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
                List<TitleContentEntity> listContentEntity = new List<TitleContentEntity>();//内容
                List<ResultEntity> listResultEntity = new List<ResultEntity>();//所有内容
                try
                {
                    for (int i = 0; i < elementTbTitle.Count; i++)
                    {
                        var elementss = elementTbTitle[i];
                        Elements elementTbChildReplies = elementss.Select("span.threadlist_rep_num").Select("span.center_text");
                        Elements elementTbUrl = elementss.Select("a.j_th_tit");
                        Debug.WriteLine(elementTbChildReplies + "/n/r" + elementTbUrl);
                        ResultEntity resultEntity = new ResultEntity();
                        if (elementTbUrl.ToString().Contains("href"))
                        {
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
                    IOFileHelper ioFileHelper = new IOFileHelper();
                    JsonHelper jsonHelper = new JsonHelper();
                    var jsonData = jsonHelper.SerializerJson(FilterListResultEntity);
                    if (jsonData == null)
                    {
                        Debug.WriteLine("写入json之前 序列化出错");
                    }
                    ///写入json 文件之前 把实体转换为json字符串
                    if (ioFileHelper.SaveJsonFile(JsonContentPath, "" + fileName + ".json", jsonData, false) == false)
                    {
                        Debug.WriteLine("文件写入出错");
                    }
                    TBTitleListService tBTitleListService = new TBTitleListService();
                    tBTitleListService.InsertTBTItleList(MySqlConn.GetMysqlConn(), FilterListResultEntity);//添加到数据库
                }
                catch (Exception e)
                {
                    Debug.WriteLine("-------CrawlerNproView Error------" + e);
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

        public async Task<GetContextModel> GetContext(string url, string pageSize = "1")
        {
            GetContextModel getContextModel = new GetContextModel();
            string JsonContentPath = ConfigurationManager.AppSettings["JsonContentPath"];
            if (url == null || pageSize == null)
            {
                //错误报告?pn=3
                //  url = @"5463733135";
            }
            var strUrl = @"https://tieba.baidu.com/p/" + url + "?pn=" + pageSize;
            Regex getAttContex = new Regex(@">(.*?)<");
            Regex regTime = new Regex(@"20\d{2}(-|\/)((0[1-9])|(1[0-2]))(-|\/)((0[1-9])|([1-2][0-9])|(3[0-1]))(T|\s)(([0-1][0-9])|(2[0-3])):([0-5][0-9])");
            string strGetConetex = null;
            string pageNumber = null;
            string repelyNumber = null;
            string fileName = "5463733135";
            CrHttpRequest crHttpRequest = new CrHttpRequest();
            strGetConetex = await crHttpRequest.SentDataAsync(HttpMethod.Post, strUrl);
            List<ContentEntity> listContent = new List<ContentEntity>();
            ContentEntity contenEntity = new ContentEntity();
            if (strGetConetex.Length > 0)
            {
                Document document = NSoup.NSoupClient.Parse(strGetConetex);
                //Elements elementTbNum = document.GetElementsByClass("card_infoNum");
                //Elements elementTbName = document.GetElementsByClass("card_title_fname");
                Elements elementTbTitle = document.Select("div.l_post").Select("div.l_post_bright").Select("div.j_l_post").Select("div.clearfix");
                //获取回复和分页数
                var tempAttribute = document.Select("li.l_reply_num").First().ToString();
                Elements pageNum = NSoup.NSoupClient.Parse(tempAttribute).Select("li.l_reply_num");
                foreach (var item in pageNum)
                {
                    var replyNumPage = NSoup.NSoupClient.Parse(item.ToString()).Select("span").ToString();
                    if (replyNumPage.Contains("style"))// 回复数
                    {
                        var tempRn = getAttContex.Match(replyNumPage).ToString();
                        repelyNumber = tempRn.Substring(1, tempRn.Length - 2);

                    }
                    if (replyNumPage.Contains("red"))//分页数
                    {
                        var tempPn = getAttContex.Match(replyNumPage).NextMatch().ToString();
                        pageNumber = tempPn.Substring(1, tempPn.Length - 2);
                    }
                }
                //this.txtContent.Text = elementTbTitle.ToString();
                foreach (var item in elementTbTitle)
                {
                    ContentEntity contenEntityTemp = new ContentEntity();
                    var data = NSoup.NSoupClient.Parse(item.ToString());
                    Debug.WriteLine(data);
                    //cc 标签
                    var content = NSoup.NSoupClient.Parse(data.ToString()).Select("cc");
                    if (content.Count == 0)
                    {
                        getContextModel.State = "300";
                        Debug.WriteLine("-------------------贴内容为空----error400-------");
                    }
                    else
                    {
                        //span  楼层 和时间
                        var span = data.Select("span.tail-info");
                        contenEntityTemp.Url = url;
                        var tempContent = Regex.Replace(content.ToString(), "<[^>]+>", "");
                        contenEntityTemp.Content = tempContent.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                        foreach (var itemspan in span)
                        {
                            if (itemspan.ToString().Contains("楼"))//可以不要
                            {
                                var tempFloor = getAttContex.Match(itemspan.ToString()).ToString();
                                var floor = tempFloor.Substring(1, tempFloor.Length - 2);
                                contenEntityTemp.Floor = floor.Trim();
                            }
                            else if (itemspan.ToString().Contains(":"))//可以不要
                            {
                                var floorTIme = regTime.Match(itemspan.ToString()).ToString();
                                contenEntityTemp.ReplyTime = floorTIme.Trim();
                            }
                            contenEntityTemp.ReplyNum = repelyNumber;
                            contenEntityTemp.PageNum = pageNumber;
                        }
                        contenEntity = contenEntityTemp;
                        listContent.Add(contenEntity);
                    }
                }
                JsonHelper jsonHelper = new JsonHelper();
                var strlistJson = jsonHelper.SerializerJson(listContent);
                IOFileHelper ioFileHelper = new IOFileHelper();
                if (pageSize == "1")
                {
                    ioFileHelper.SaveJsonFile(JsonContentPath, "" + fileName + ".json", strlistJson);
                }
                else
                {
                    ioFileHelper.SaveJsonFile(JsonContentPath, "" + fileName + ".json", strlistJson, true);
                }
                getContextModel.Flag = true;
                getContextModel.State = "200";
                getContextModel.Total = repelyNumber;
                getContextModel.FullPageSIze = pageNumber;
                var list = jsonHelper.DeserializeJsonTo<List<ContentEntity>>(strlistJson);
                return getContextModel;
            }
            else
            {
                getContextModel.Flag = false;
                return getContextModel;
            }
        }

#if DEBUG
        /// <summary>
        /// 插入索引 手动执行
        /// </summary>
        public void InsertKeyWorld()
        {
            string SearchIndex = ConfigurationManager.AppSettings["SearchIndex"];
            KeyWorldService keyWorldService = new KeyWorldService();
            SysTBKeyWordDAL tBKeywordDAL = new SysTBKeyWordDAL();
            var list = keyWorldService.getTextContent(SearchIndex);
            var result = tBKeywordDAL.InsertSysKeyworld(MySqlConn.GetMysqlConn(), list);
            if (result == 0)
            {
                Debug.WriteLine("insert index data failed");
            }
        }

        public List<KeyWorldEntity> SelectKeyWorld()
        {
            SysTBKeyWordDAL tBKeyWordDAL = new SysTBKeyWordDAL();
            var list = tBKeyWordDAL.SelectSysKeyworld(MySqlConn.GetMysqlConn(), null);
            if (list.Count < 0)
            {
                Debug.WriteLine("select index table failed");
                return null;
            }
            return list;
        }

        /// <summary>
        /// 读取本地txt文件 把贴吧名字写入到数据库
        /// </summary>
        public void InsertTieBarName()
        {
            string tBName = ConfigurationManager.AppSettings["TBName"];
            TBNameServices tBNameServices = new TBNameServices();
            var list = tBNameServices.InsertJson(tBName);
            if (list == false)
            {
                Debug.WriteLine("insert index data failed");
            }
        }

        ///java sys
        /// <summary>
        /// read database  to local json file
        /// </summary>
        public void SaveTBNameToJson(Uri JsonPath = null)
        {
            //读取数据库
            SysTBNameDAL sysTBNAMEDAL = new SysTBNameDAL();
            var listTBName = sysTBNAMEDAL.SelectTBName(MySqlConn.GetMysqlConn(), null);
            //转换json 
            JsonHelper jsonHelper = new JsonHelper();
            var strJson = jsonHelper.SerializerJson(listTBName);
            //保存文件
            IOFileHelper iOFileHelper = new IOFileHelper();
            string JsonContentPath = ConfigurationManager.AppSettings["JsonContentPath"];
            iOFileHelper.SaveJsonFile(JsonContentPath, "BarName.json", strJson);//文件名写死
        }
#endif
        /// <summary>
        /// 读取applocal  json 文件贴吧Name
        /// </summary>
        /// <param name="JsonPath"></param>
        public static List<BarNameEntity> ReadTBNameFile(Uri JsonPath = null)
        {
            try
            {
                IOFileHelper iOFileHelper = new IOFileHelper();
                var strJson = iOFileHelper.ReadJsonFileToString(@"/CrawlerNproData/BarName.json");
                JsonHelper jsonHelper = new JsonHelper();
                return jsonHelper.DeserializeJsonTo<List<BarNameEntity>>(strJson);
            }
            catch 
            {
                Debug.WriteLine("-------cls-CrawlerNproView------func ReadTBNameFile----");
            }
            return null;
        }
    }
}

