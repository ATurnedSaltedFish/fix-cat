using CrawlerNpro.entity;
using CrawlerNpro.ServiceInput;
using CrawlerNpro.sqlDAL;
using CrawlerNpro.toolkit;
using CrawlerNpro.ViewControl;
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
            CrawlerNproView crawlerNproView = new CrawlerNproView();
            crawlerNproView.SearchStart(url,);
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
                if (ioFileHelper.SaveJsonFile(@"C:\Users\LcAns\Desktop\jsonFileSave\", "" + fileName + ".json", strlistJson) ==false)
                {
                    Debug.WriteLine("文件写入出错");
                } 
                var list=jsonHelper.DeserializeJsonTo<List<ContentEntity>>(strlistJson);
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
