using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.entity
{
    public class ContentEntity
    {
        private int id;
        //url
        private string url;
        //content
        private string content;
        //titleCreateTime
        private string titleCreateTime;
        //repear time
        private string replyTime;
        //floor
        private string floor;
        //replyNum
        private string replyNum;
        //pageNum
        private string pageNum;
        private string createTime;//添加字段sqlite
        private string updateTime;//添加字段sqlite

        private string pageSize;//页数
        private string visiable;
        private int createCode;
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

        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
            }
        }
 

        public string ReplyTime
        {
            get
            {
                return replyTime;
            }

            set
            {
                replyTime = value;
            }
        }

        public string Floor
        {
            get
            {
                return floor;
            }

            set
            {
                floor = value;
            }
        }

        public string ReplyNum
        {
            get
            {
                return replyNum;
            }

            set
            {
                replyNum = value;
            }
        }

        public string PageNum
        {
            get
            {
                return pageNum;
            }

            set
            {
                pageNum = value;
            }
        }

        public string Visiable
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

        public int CreateCode
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

        public string TitleCreateTime
        {
            get
            {
                return titleCreateTime;
            }

            set
            {
                titleCreateTime = value;
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

        public string PageSize
        {
            get
            {
                return pageSize;
            }

            set
            {
                pageSize = value;
            }
        }
        //infloor
    }
}
