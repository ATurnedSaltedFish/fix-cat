using CrawlerNpro.entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.toolkit
{
    public class CompareHelper
    {
        public bool Compare<T>(string strTitle, List<T> realTitle, string key = "KeyWorld") where T : class
        {
            bool flag = false;
            foreach (var item in realTitle)
            {
                var data = item.GetType().GetProperty(key).GetValue(item, null);
                bool result = strTitle.Contains(data.ToString());
                if (result == true)
                {
                    flag = true;
                    return flag;
                }
            }
            return flag;
        }
    }
}
