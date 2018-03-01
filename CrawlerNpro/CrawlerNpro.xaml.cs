using CrawlerNpro.entity;
using CrawlerNpro.EnumPro;
using CrawlerNpro.Model;
using CrawlerNpro.ServiceInput;
using CrawlerNpro.sqlDAL;
using CrawlerNpro.SqlDAL;
using CrawlerNpro.toolkit;
using CrawlerNpro.UIControl;
using CrawlerNpro.ViewControl;
using MySql.Data.MySqlClient;
using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
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
            CrawlerNproView cnv = new CrawlerNproView();
            Init();
            // LocTitleListDAL loct = new LocTitleListDAL();
            // loct.SelectLocalTBTitle();
            // cnv.InsertTieBarName();
            //  var datas = cnv.ReadTBNameFile();
            //   cnv.InsertTieBarName();
            //   cnv.SaveTBNameToJson();
            //  cnv.InsertKeyWorld();
            var data = CrawlerNproView.ReadTBNameFile();
            lsturl.ItemsSource = data;

            //lsturl.ItemsSource = new List<SearchlistEntity> {
            //   //https://tieba.baidu.com/f?kw=%E5%BF%B5%E7%A0%B4&ie=utf-8&pn=100
            //   new SearchlistEntity("念破","https://tieba.baidu.com/f?kw=%E5%BF%B5%E7%A0%B4&ie=utf-8","1"),
            //   new SearchlistEntity("剑网三交易","https://tieba.baidu.com/f?kw=%E5%89%91%E7%BD%91%E4%B8%89%E4%BA%A4%E6%98%93&ie=utf-8","2"),
            //   new SearchlistEntity("剑网三账号交易","https://tieba.baidu.com/f?kw=%E5%89%91%E7%BD%91%E4%B8%89%E8%B4%A6%E5%8F%B7%E4%BA%A4%E6%98%93&ie=utf-8","3"),
            //   new SearchlistEntity("五鸢","https://tieba.baidu.com/f?kw=%E4%BA%94%E9%B8%A2&ie=utf-8","4"),
            //   new SearchlistEntity("唯满侠","https://tieba.baidu.com/f?kw=%E5%94%AF%E6%BB%A1%E4%BE%A0&ie=utf-8","5"),
            // };
        }

        /// <summary>
        /// 初始化加载数据
        /// </summary>
        public void Init()
        {
            ObservableCollection<ResultEntity> tbTitle = new ObservableCollection<ResultEntity>();
            LocTitleListService locTitleListService = new LocTitleListService();
            var totalNum = locTitleListService.SelectAllCount();
            var data = locTitleListService.SelectLocalTitleList("50", "0");
            this.gridTitle.ItemsSource = data;
            Page(totalNum.ToString(), "50", "1", "1");
        }

        /// <summary>
        /// 开始获取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnGetData(object sender, RoutedEventArgs e)
        {
            Elements elements;
            string strGetConetex = null;
            var content = this.txtUri.Text.Trim();
            if (content == null)
            {
                content = lsturl.Items[0].ToString();
                txtUri.Text = content;
            }
            CrawlerNproView crawlerNproView = new CrawlerNproView();
            var url = crawlerNproView.UrlFiler(content);
            int type = (int)ViewEnum.searchEnum.WriteSQLite;
            crawlerNproView.SearchStart(url, content, type);//搜索开始
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

        #region 贴吧内容
        /// <summary>
        /// 获取贴吧内容 按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnGetContexData(object sender, RoutedEventArgs e)
        {
            
            string pagesize = "1";
            var flag = await GetTitleListByJSON(pagesize);
        }

        /// <summary>
        ///获取贴吧内容
        /// </summary>
        /// <param name="url"></param>
        public async Task<bool> GetContextData(string url, string pagesize)
        {
            bool flag;
            CrawlerNproView crawlerNproView = new CrawlerNproView();
            var resultFrist = await crawlerNproView.GetContext(url);
            flag = resultFrist.Flag;
            //如果返回实体 的值不为1 就进行循环
            var count = Convert.ToInt32(resultFrist.FullPageSIze);
            if (count > 1)
            {
                for (int i = 2; i < count; i++)
                {
                    var resultOther = await crawlerNproView.GetContext(url);
                    flag = resultOther.Flag;
                }
            }
            return flag;
        }

        /// <summary>
        /// 读取json文件获取贴吧标题列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="filePath"></param>
        /// <returns></returns> 
        public async Task<bool> GetTitleListByJSON(string pagesize)
        {
            bool flag = false;
            var JsonContentPath = ConfigurationManager.AppSettings["JsonContentPath"];//E:\CrawlerNproData\jsonFileSave\
            IOFileHelper iOFileHelper = new IOFileHelper();
            var fileNames = iOFileHelper.GetFileNames(JsonContentPath);
            if (fileNames == null)
            {
                return false;
            }
            foreach (var fileName in fileNames)
            {
                var file = fileName.ToString();
                var result = await GetContextData(JsonContentPath + file + ".json", pagesize);
                flag = result;
            }
            return flag;
        }
        /// <summary>
        /// 读取Sqlite获取贴吧标题列表
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetTitleListBySqlite()
        {
            LocTitleListService locContentListService = new LocTitleListService();
            locContentListService.SelectLocalTitleList("50","0");
            return false;
        }

        /// <summary>
        ///删除贴吧内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteContent_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 查内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectContent_Click(object sender, RoutedEventArgs e)
        {
            this.gridContent.ItemsSource = "";
        }

        /// <summary>
        /// 内容  回退按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBackContentPage_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 下一页按钮内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextContentPage_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion


        private void btnSelectTitle_Click(object sender, RoutedEventArgs e)
        {
            //ObservableCollection<ResultEntity> tbTitle = new ObservableCollection<ResultEntity>();
            //LocTitleListService locTitleListService = new LocTitleListService();
            //var data = locTitleListService.SelectLocalTitleList("50","0");
            //this.gridTitle.ItemsSource = data;
            // PageControl pc = new PageControl();
            // var v = pc.Content as UserControl;

            //var btnback = v.FindName("BtnBack") as Button;
            // var btnnext = v.FindName("BtnNext") as Button;
            //btnback.RaiseEvent(new RoutedEventArgs(e.RoutedEvent));
            //btnnext.RaiseEvent(new RoutedEventArgs(e.RoutedEvent));
        }

        #region 分页控件
        #region PageEntity
        public static class PageEntity
        {
            /// <summary>
            /// 跳转页面
            /// </summary>
            private static int turnPage;
            /// <summary>
            /// 总页面数
            /// </summary>
            private static int totalPage;
            /// <summary>
            /// 数据总数
            /// </summary>
            private static int totalNum;
            /// <summary>
            /// 总行数
            /// </summary>
            private static int totalRowsPage;
            /// <summary>
            /// 剩下的页数
            /// </summary>
            private static int remainPage;
            /// <summary>
            /// 当前页面
            /// </summary>
            private static int nowPage;

            public static int TurnPage
            {
                get
                {
                    return turnPage;
                }

                set
                {
                    turnPage = value;
                }
            }

            public static int TotalPage
            {
                get
                {
                    return totalPage;
                }

                set
                {
                    totalPage = value;
                }
            }

            public static int TotalRowsPage
            {
                get
                {
                    return totalRowsPage;
                }

                set
                {
                    totalRowsPage = value;
                }
            }

            public static int NowPage
            {
                get
                {
                    return nowPage;
                }

                set
                {
                    nowPage = value;
                }
            }

            public static int RemainPage
            {
                get
                {
                    return remainPage;
                }

                set
                {
                    remainPage = value;
                }
            }

            public static int TotalNum
            {
                get
                {
                    return totalNum;
                }

                set
                {
                    totalNum = value;
                }
            }
        }
        #endregion
        /// <summary>
        /// 用户空间分页初始化
        /// </summary>
        /// <param name="totalNum">数据总数</param>
        /// <param name="rowsNum">每页显示多少条数据</param>
        /// <param name="nowPage">当前的页面 第N页</param>
        /// <param name="turnPage">跳转页面数</param>
        public void Page(string totalNum, string rowsNum, string nowPage, string turnPage)
        {
            var intTotalNum = Convert.ToInt32(totalNum);
            var intRowsNum = Convert.ToInt32(rowsNum);
            var intNowPage = Convert.ToInt32(nowPage);
            var intTurnPage = Convert.ToInt32(turnPage);
            var temp = (intTotalNum - intNowPage * intRowsNum) / intRowsNum;//(131-50x1)/50=0
            float tempAllpage = intTotalNum / intRowsNum;
            var tempNowPage = (intTotalNum - intNowPage * intRowsNum) % intRowsNum; //余下的数据
            var remainPage = Math.Round(Convert.ToDouble(tempNowPage), 0);//余下的页面
            var allpage = Math.Round(tempAllpage, 0);

            if (temp <= 0)
            {
                intNowPage = 1;
                if (tempNowPage == 0)//页面值小于分页规定的数据
                {
                    intNowPage = tempNowPage;
                }
                else
                {
                    intNowPage = tempNowPage + 1;//取余的数据
                }
            }
            else
            {
                allpage = tempAllpage + 1;
            }
            PageEntity.TotalPage = Convert.ToInt32(allpage);
            PageEntity.NowPage = intNowPage;
            PageEntity.TurnPage = intTurnPage;//Convert.ToInt32(turnPage);
            PageEntity.TotalNum = intTotalNum;
            PageEntity.RemainPage = tempNowPage;
            PageEntity.TotalRowsPage = intRowsNum;
            //  PageEntity.RemainPage = Convert.ToInt32(remainPage);
            //当前的页面
            this.lblNowPage.Content = PageEntity.NowPage;
            //跳转页面
            this.txtTurnPage.Text = PageEntity.TurnPage.ToString();
            //总页数
            this.lblTotalPage.Content = PageEntity.TotalPage;
            //总行数
            this.lblTotalRows.Content = PageEntity.TotalRowsPage;
        }

        //未测试
        /// <summary>
        /// 遍历grid 选中的id
        /// </summary>
        private List<string> GetGridInfo()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < gridTitle.Items.Count; i++)
            {
                var data = this.gridTitle.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow dataGridRow = (DataGridRow)data;
                if (dataGridRow != null)
                {
                    FrameworkElement frameworkElement = gridTitle.Columns[0].GetCellContent(dataGridRow);
                    FrameworkElement id = gridTitle.Columns[1].GetCellContent(dataGridRow);
                    CheckBox checkbox = (CheckBox)frameworkElement;
                    var rowDataContext = dataGridRow.DataContext as ResultEntity;
                    if (checkbox.IsChecked == true)
                    {
                        string result = rowDataContext.Id.ToString();
                        list.Add(result);
                    }
                }
            }
            return list;
        }


        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            var totalPage = PageEntity.TotalPage;
            var rowsNum = PageEntity.NowPage;
            var totalRowsPage = PageEntity.TotalRowsPage;
            var remain = PageEntity.RemainPage;
            //判断页面最后一页
            var nowPageSize = (Convert.ToInt32(lblNowPage.Content.ToString()));//当前页面页数
            var data = nowPageSize * totalRowsPage;                                           //var nowPageSizeNum = nowPageSize * rowsNum;
            if (nowPageSize >= totalPage - 1)// 3>=3
            {
                nowPageSize = totalPage;
                this.lblNowPage.Content = totalPage;
            }
            else if (nowPageSize == totalPage) //3<3+1
            {
                if (remain > 1)
                {
                    data = remain;
                }
            }
            LocTitleListService locTitleListService = new LocTitleListService();
            this.gridTitle.ItemsSource = locTitleListService.SelectLocalTitleList(totalRowsPage.ToString(), data.ToString());
            nowPageSize = PageEntity.NowPage + 1;
            this.lblNowPage.Content = nowPageSize;
            PageEntity.NowPage = nowPageSize;
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBackPage_Click(object sender, RoutedEventArgs e)
        {
            //判断第一页不为0页面66
            var nowPageSize = Convert.ToInt32(lblNowPage.Content.ToString());//当前页面页数
            var totalPage = PageEntity.TotalPage;//3
            var rowsNum = PageEntity.NowPage;//1234
            var totalNum = PageEntity.TotalNum;//131
            var totalRowsPage = PageEntity.TotalRowsPage;//每页显50少条
            var remainPage = PageEntity.RemainPage; //31
            LocTitleListService locTitleListService = new LocTitleListService();
            if (nowPageSize > totalPage)
            {

                PageEntity.NowPage = nowPageSize - 1;
                this.lblNowPage.Content = PageEntity.NowPage;
                this.gridTitle.ItemsSource = locTitleListService.SelectLocalTitleList(totalRowsPage.ToString(), (totalRowsPage * (PageEntity.NowPage - 1)).ToString());
            }
            else if (nowPageSize == totalPage)
            {
                PageEntity.NowPage = nowPageSize - 1;
                this.lblNowPage.Content = PageEntity.NowPage;
                this.gridTitle.ItemsSource = locTitleListService.SelectLocalTitleList(totalRowsPage.ToString(), (totalRowsPage * (PageEntity.NowPage - 1)).ToString());
            }
            else if (nowPageSize == 0)
            {
                PageEntity.NowPage = 1;
                this.lblNowPage.Content = PageEntity.NowPage;
            }
            else if (nowPageSize < totalPage && nowPageSize >= 1)
            {
                PageEntity.NowPage = nowPageSize - 1;
                this.lblNowPage.Content = PageEntity.NowPage;
                this.gridTitle.ItemsSource = locTitleListService.SelectLocalTitleList(totalRowsPage.ToString(), (totalRowsPage * (PageEntity.NowPage - 1)).ToString());
            }


            //    ///***////
            //    //var nowPageSizeNum = nowPageSize * rowsNum;
            //    LocTitleListService locTitleListService = new LocTitleListService();
            ////if (nowPageSize==1)
            ////{
            ////    nowPageSize = 1;
            ////    PageEntity.NowPage = nowPageSize;
            ////    this.lblNowPage.Content = nowPageSize;
            ////}
            // if (nowPageSize==totalPage)//3==3  4
            // {
            //   // nowPageSize = (totalNum - remainPage) / totalRowsPage;//100/50
            //    PageEntity.NowPage = nowPageSize-1;//2
            //    this.lblNowPage.Content = PageEntity.NowPage;
            //}
            //else if (nowPageSize>totalPage) 
            //{
            //    PageEntity.NowPage= nowPageSize - 1;
            //    this.lblNowPage.Content = PageEntity.NowPage;
            //}
            //else if (nowPageSize<totalPage)
            //{
            //    if (nowPageSize==1)
            //    {
            //        nowPageSize = 0;
            //        PageEntity.NowPage = nowPageSize;
            //        this.lblNowPage.Content = PageEntity.NowPage;
            //    }
            //    else
            //    {
            //        PageEntity.NowPage = nowPageSize-1;
            //        this.lblNowPage.Content = PageEntity.NowPage;
            //    }
            //}
            //this.gridTitle.ItemsSource = locTitleListService.SelectLocalTitleList(totalRowsPage.ToString(), (totalRowsPage * PageEntity.NowPage).ToString());
        }
        #endregion

        /// <summary>
        /// 业务手动删除标题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteTitle_Click(object sender, RoutedEventArgs e)
        {
            LocTitleListService locTitleListService = new LocTitleListService();
            var strdata = GetGridInfo().ToArray();
            var result = locTitleListService.DelUpdateTBTitl(strdata);
            if (result == 2)
            {
                var pageSize = PageEntity.TotalRowsPage;
                var nowPage = PageEntity.NowPage;
                this.gridTitle.ItemsSource = locTitleListService.SelectLocalTitleList(pageSize.ToString(), ((nowPage - 1) * pageSize).ToString());//搜索开始
            }
        }

        /// <summary>
        /// 设置任务时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetWorkTime_Click(object sender, RoutedEventArgs e)
        {
        

        }





    }
}
