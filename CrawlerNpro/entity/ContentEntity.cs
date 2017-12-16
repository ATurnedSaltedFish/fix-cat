using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.entity
{
    public class ContentEntity
    {
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
        //infloor
    }
}
