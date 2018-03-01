using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if DEBUG
namespace CrawlerNpro.entity
{
 public class BarNameEntity
    {
        private string barName;
        private string url;
        private string type;
        private int hot;
        private int visiable;

        /// <summary>
        /// baidu bar Name
        /// </summary>
        public string BarName
        {
            get
            {
                return barName;
            }

            set
            {
                barName = value;
            }
        }
        
        /// <summary>
        /// baidubar Url
        /// </summary>
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

        /// <summary>
        ///bar  type  
        /// </summary>
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        /// <summary>
        /// hot leve
        /// </summary>
        public int Hot
        {
            get
            {
                return hot;
            }

            set
            {
                hot = value;
            }
        }

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
    }
}
#endif