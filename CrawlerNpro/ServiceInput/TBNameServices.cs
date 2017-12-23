using CrawlerNpro.entity;
using CrawlerNpro.SqlDAL;
using CrawlerNpro.toolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.ServiceInput
{
  public  class TBNameServices
    {
        /// <summary>
        /// read json file to list
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<BarNameEntity> getJsonContent(string filepath)
        {
            List<BarNameEntity> listkeyWorldEntity = new List<BarNameEntity>();
            try
            {
                if (string.IsNullOrEmpty(filepath))
                {
                    return null;
                }
                IOFileHelper iOFileHelper = new IOFileHelper();
                var strContext = iOFileHelper.ReadTxtFile(filepath);
                var byteData = strContext.Split('-');
                UrlCodeConvert urlCodeConvert = new UrlCodeConvert();
                foreach (var item in byteData)
                {
                    BarNameEntity barNameEntity = new BarNameEntity();
                    barNameEntity.BarName = item.Trim();
                    barNameEntity.Url = urlCodeConvert.EncodeUrl(item.Trim());
                    listkeyWorldEntity.Add(barNameEntity);
                }
                return listkeyWorldEntity;
            }
            catch
            {
                Debug.WriteLine("KeyWorldService");
            }
            return listkeyWorldEntity;
        }

        /// <summary>
        /// insert json data
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public bool InsertJson(string filepath)
        {
            try
            {
                SysTBNameDAL sysTBNameDAL = new SysTBNameDAL();
                var list = getJsonContent(filepath);
                if (list == null)
                {
                    return false;
                }
                var data = sysTBNameDAL.InsertTBName(MySqlConn.GetMysqlConn(), list);
                if (data == 0)
                {
                    return false;
                }
            }
            catch 
            {
                Debug.Write("--------TBNameServices Error---------");
            }
            return true;
        }
    }
}
