using CrawlerNpro.entity;
using CrawlerNpro.SqlDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.ServiceInput
{
    public class LocTitleListService
    {
       // 查询数据返回datatable
        public DataTable SelectLocalBarName()
        {
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            var data = locTitleListDAL.SelectLocalTBTitle();
            if (data == null)
            {
                return null;
            }
            return data;
        }
        /// <summary>
        /// 将贴吧标题插入到本地数据库
        /// </summary>
        /// <param name="listTitleContentEntity">标题</param>
        /// <returns></returns>
        public int InsertLocalTBTitle(List<TitleContentEntity> listTitleContentEntity)
        {
            if (listTitleContentEntity.Count <= 0)
            {
                return 0;
            }
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            return locTitleListDAL.InsertLocalTBTitle(listTitleContentEntity);
        }

    }
}
