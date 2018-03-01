using CrawlerNpro.ServiceInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace CrawlerNpro.UIControl
{
    public delegate void ClickEventHandler(object sender, EventArgs e);
    /// <summary>
    /// PageControl.xaml 的交互逻辑
    /// </summary>
    public partial class PageControl : UserControl
    {
       // public event ClickEventHandler Click;

        //protected void OnClick(EventArgs e)
        //{
        //    Click?.Invoke(this, e);
        //}
        public PageControl()
        {
            InitializeComponent();
            Page("300","50","1","1");
        }
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
        public void Page(string totalNum, string rowsNum, string nowPage,string turnPage)
        {
            var intTotalNum = Convert.ToInt32(totalNum);
            var intRowsNum = Convert.ToInt32(rowsNum);
            var intNowPage = Convert.ToInt32(nowPage);
            var intTurnPage = Convert.ToInt32(turnPage);
            float temp=(intTotalNum - intNowPage* intRowsNum) / intRowsNum;//(50-50x1)/50=0
            float tempAllpage = intTotalNum / intRowsNum;
            var tempNowPage = (intTotalNum - intNowPage* intRowsNum) % intRowsNum; //余下的数据
            var remainPage=Math.Round(temp, 0);//余下的页面
            var allpage = Math.Round(tempAllpage, 0);
   
            if (remainPage <= 0)
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
            PageEntity.TotalPage = Convert.ToInt32(allpage);
            PageEntity.NowPage = intNowPage;
            PageEntity.TurnPage = Convert.ToInt32(turnPage);
            PageEntity.TotalNum = intTotalNum;
            PageEntity.TotalRowsPage = intRowsNum;
            PageEntity.RemainPage = Convert.ToInt32(remainPage);
            //当前的页面
            this.lblNowPage.Content = PageEntity.NowPage;
            //跳转页面
            this.txtTurnPage.Text = PageEntity.TurnPage.ToString() ;
            //总页数
            this.lblTotalPage.Content = PageEntity.TotalPage;
            //总行数
            this.lblTotalRows.Content = PageEntity.TotalRowsPage;
    
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_NextPage(object sender, RoutedEventArgs e)
        {
           // this.OnClick(e);
            var totalPage = PageEntity.TotalPage;
            var rowsNum = PageEntity.NowPage;
            //判断页面最后一页
            var nowPageSize = (Convert.ToInt32(lblNowPage.Content.ToString()));//当前页面页数
            var nowPageSizeNum = nowPageSize * rowsNum;
            if (nowPageSize < 1)
            {
                nowPageSize = 1;
            }
           else if (nowPageSizeNum >= totalPage)
            {
                nowPageSize = totalPage;
            }
            else
            {
                nowPageSize = nowPageSize + 1;
            }
            PageEntity.NowPage = nowPageSize;
            LocTitleListService locTitleListService = new LocTitleListService();
            var data = locTitleListService.SelectLocalTitleList(rowsNum.ToString(),nowPageSize.ToString());//2,2
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_BackPage(object sender, RoutedEventArgs e)
        {
           // this.OnClick(e);
            //判断第一页不为0页面
            var nowPageSize = Convert.ToInt32(lblNowPage.Content.ToString());//当前页面页数
            if (nowPageSize <= 1)
            {
                nowPageSize = 1;
            }
        }
    }
}
