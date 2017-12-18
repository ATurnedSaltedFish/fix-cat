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
            //crawlerNproView.SearchStart(url,fileName);
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
         
        }

        public async void GetContexData(string url,string urls)
        {
            CrawlerNproView crawlerNproView = new CrawlerNproView();
          var result= await crawlerNproView.GetContext(url);
            //如果返回实体 的值不为1 就进行循环
            var count = Convert.ToInt32(result.FullPageSIze);
            if (count > 1)
            {
                for (int i = 2; i < count; i++)
                {
               await crawlerNproView.GetContext(url);
                }
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
