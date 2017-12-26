using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.entity
{
    public class TitleContentEntity : ResultEntity
    {
        private string title;
        private string url;
        private string createCode;//添加字段
        private string createTime;//添加字段

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
    }
}
