using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro
{
  public  class ResultEntity
    {
        private int id;//sqlite主键
        private string replies;
        private int intreplies;
        private string title;
        private string url;
        private string uname;
        private string uid;
        private int visiable;
        private string searchTimes;//查询重复次数 用于分析
        private string createCode;//添加字段sqlite
        private string createTime;//添加字段sqlite
        private string updateTime;//添加字段sqlite
        private int icode;//url校验int
        private bool isChecked;//控件选择

        public string Replies
        {
            get
            {
                return replies;
            }

            set
            {
                replies = value;
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

        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                url = value;
            }
        }

        public string Uname
        {
            get
            {
                return uname;
            }

            set
            {
                uname = value;
            }
        }

        public string Uid
        {
            get
            {
                return uid;
            }

            set
            {
                uid = value;
            }
        }
        public string CreateTime
        {
            get
            {
                return createTime;
            }

            set
            {
                createTime = value;
            }
        }

        public string CreateCode
        {
            get
            {
                return createCode;
            }

            set
            {
                createCode = value;
            }
        }

        public string UpdateTime
        {
            get
            {
                return updateTime;
            }

            set
            {
                updateTime = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public int Intreplies
        {
            get
            {
                return intreplies;
            }

            set
            {
                intreplies = value;
            }
        }

        /// <summary>
        /// 1显示 0删除状态
        /// </summary>
        public int Visiable
        {
            get
            {
                return visiable;
            }

            set
            {
                visiable = value;
            }
        }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
            }
        }

        public string SearchTimes
        {
            get
            {
                return searchTimes;
            }

            set
            {
                searchTimes = value;
            }
        }

        /// <summary>
        /// id号转换为url
        /// </summary>
        public int Icode
        {
            get
            {
                return icode;
            }

            set
            {
                icode = value;
            }
        }
    }
}
