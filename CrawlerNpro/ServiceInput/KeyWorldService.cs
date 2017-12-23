using CrawlerNpro.entity;
using CrawlerNpro.toolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.ServiceInput
{
 public   class KeyWorldService
    {
        /// <summary>
        /// get keyworld content from text file
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<KeyWorldEntity> getTextContent(string url)
        {
            List<KeyWorldEntity> listkeyWorldEntity = new List<KeyWorldEntity>();
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return null;
                }
                IOFileHelper iOFileHelper = new IOFileHelper();
                var strContext = iOFileHelper.ReadTxtFile(url);
                var byteData = strContext.Split('-');
                foreach (var item in byteData)
                {
                    KeyWorldEntity keyWorldEntity = new KeyWorldEntity();
                    keyWorldEntity.KeyWorld = item.Trim();
                    listkeyWorldEntity.Add(keyWorldEntity);
                }
                return listkeyWorldEntity;
            }
            catch
            {
                Debug.WriteLine("KeyWorldService");
            }
            return listkeyWorldEntity;
        }
    }
}
