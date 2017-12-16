using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro
{
  public class SearchlistEntity
    {
        private string title;
        private string searchContent;
        private string checkCode;

        /// <summary>
        /// 查询内容
        /// </summary>
        public string SearchContent
        {
            get
            {
                return searchContent;
            }

            set
            {
                searchContent = value;
            }
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public string CheckCode
        {
            get
            {
                return checkCode;
            }

            set
            {
                checkCode = value;
            }

        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        public SearchlistEntity(string title, string searchContent, string checkCode)
        {
            this.SearchContent = searchContent;
            this.CheckCode = checkCode;
            this.Title = title;
        }
    }
}
