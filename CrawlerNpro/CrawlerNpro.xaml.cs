using CrawlerNpro.entity;
using CrawlerNpro.ServiceInput;
using CrawlerNpro.sqlDAL;
using CrawlerNpro.toolkit;
using MySql.Data.MySqlClient;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrawlerNpro
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CrawlerMain : Window
    {
        private SearchlistEntity searchlistEntity;
  
        public CrawlerMain()
        {
            InitializeComponent();
            lsturl.ItemsSource = new List<SearchlistEntity> {
               //https://tieba.baidu.com/f?kw=%E5%BF%B5%E7%A0%B4&ie=utf-8&pn=100
               new SearchlistEntity("念破","https://tieba.baidu.com/f?kw=%E5%BF%B5%E7%A0%B4&ie=utf-8","1"),
               new SearchlistEntity("剑网三交易","https://tieba.baidu.com/f?kw=%E5%89%91%E7%BD%91%E4%B8%89%E4%BA%A4%E6%98%93&ie=utf-8","2"),
               new SearchlistEntity("剑网三账号交易","https://tieba.baidu.com/f?kw=%E5%89%91%E7%BD%91%E4%B8%89%E8%B4%A6%E5%8F%B7%E4%BA%A4%E6%98%93&ie=utf-8","3"),
               new SearchlistEntity("五鸢","https://tieba.baidu.com/f?kw=%E4%BA%94%E9%B8%A2&ie=utf-8","4"),
               new SearchlistEntity("唯满侠","https://tieba.baidu.com/f?kw=%E5%94%AF%E6%BB%A1%E4%BE%A0&ie=utf-8","5"),
            };
        }

        private async void BtnGetData(object sender, RoutedEventArgs e)
        {
            Elements elements;
            string strGetConetex = null;
            var url = this.txtUri.Text.Trim();
            if (url == null)
            {
                url = lsturl.Items[0].ToString();
                txtUri.Text = url;
            }
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
                this.txtContent.Text = elementTbTitle.ToString();
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
                            var tempResultUrl = regUrl.Match(elementTbUrl.ToString()).ToString();
                            var resultUrl = tempResultUrl.Substring(3, tempResultUrl.Length-3);
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
                    var listkeyWorld=SelectKeyWorld();
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

        private void checkHistory(object sender, SelectionChangedEventArgs e)
        {
            if (this.lsturl.SelectedIndex > -1)
            {
                SearchlistEntity searchlistEntity;
                searchlistEntity = (SearchlistEntity)this.lsturl.SelectedItem;
                this.txtUri.Text = searchlistEntity.SearchContent;
            }
        }

        /// <summary>
        /// get conetext  without filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnGetContexData(object sender, RoutedEventArgs e)
        {
            Regex getAttContex = new Regex(@">(.*?)<");
            Regex regTime = new Regex(@"20\d{2}(-|\/)((0[1-9])|(1[0-2]))(-|\/)((0[1-9])|([1-2][0-9])|(3[0-1]))(T|\s)(([0-1][0-9])|(2[0-3])):([0-5][0-9])");
            string strGetConetex = null;
            string pageNumber = null;
            string repelyNumber = null;
            var url = @"https://tieba.baidu.com/p/5463733135";
            string fileName = "5463733135";
            CrHttpRequest crHttpRequest = new CrHttpRequest();
            strGetConetex = await crHttpRequest.SentDataAsync(HttpMethod.Post, url);
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
                     var tempRn  = getAttContex.Match(replyNumPage).ToString();
                        repelyNumber= tempRn.Substring(1, tempRn.Length - 2);
                    }
                    if(replyNumPage.Contains("red"))//分页数
                        {
                        var tempPn = getAttContex.Match(replyNumPage).NextMatch().ToString();
                        pageNumber = tempPn.Substring(1, tempPn.Length - 2);
                    }
                }
                this.txtContent.Text= elementTbTitle.ToString();
                foreach (var item in elementTbTitle)
                {
                    ContentEntity contenEntityTemp = new ContentEntity();
                    var data = NSoup.NSoupClient.Parse(item.ToString());
                    Debug.WriteLine(data);
                    //cc 标签
                    var content = NSoup.NSoupClient.Parse(data.ToString()).Select("cc");
                    if (content.Count==0)
                    {
                        Debug.WriteLine("-------------------贴内容为空-----------");
                    }
                    else
                    {
                    //span  楼层 和时间
                    var span = data.Select("span.tail-info");
                    contenEntityTemp.Url = url;
                      var tempContent = Regex.Replace(content.ToString(), "<[^>]+>", "");
                        contenEntityTemp.Content = tempContent;
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
                var strlistjson = jsonHelper.SerializerJson(listContent);
                IOFileHelper ioFileHelper = new IOFileHelper();
                if (ioFileHelper.SaveJsonFile(@"C:\Users\xx\Desktop\jsonFileSave", "" + fileName + ".json", strlistjson)==false)
                {
                    Debug.WriteLine("文件写入出错");
                } 
                var aa=jsonHelper.DeserializeJsonTo<List<ContentEntity>>(strlistjson);
                var aas= "11";
            }
        }
#if DEBUG
        public void InsertKeyWorld()
        {
            string txtFileUrl = @"C:\Users\LcAns\Desktop\SearchIndex.txt";
            KeyWorldService keyWorldService = new KeyWorldService();
            TBKeyWordDAL tBKeywordDAL = new TBKeyWordDAL();
            var list = keyWorldService.getTextContent(txtFileUrl);
          var result=tBKeywordDAL.InsertSysKeyworld(MySqlConn.GetMysqlConn(), list);
            if (result==0)
            {
                Debug.WriteLine("insert index data failed");
            }
        }

        public List<KeyWorldEntity> SelectKeyWorld()
        {
            TBKeyWordDAL tBKeyWordDAL = new TBKeyWordDAL();
           var list= tBKeyWordDAL.SelectSysKeyworld(MySqlConn.GetMysqlConn(), null);
            if (list.Count<0)
            {
                Debug.WriteLine("select index table failed");
                return null;
            }
          return  list;
        }

#endif
    }
}
