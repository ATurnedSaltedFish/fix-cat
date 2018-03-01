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
    public class LocContentListService
    {
        public DataTable SelectLocalContent(string rows = "50", string startIndex = "0")
        {
            LocContentListDAL locContentListDAL = new LocContentListDAL();
            return locContentListDAL.SelectLocalContent(rows, startIndex);
        }
        //修改好 未测试
        /// <summary>
        /// 查询tb内容返回集合
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public List<ContentEntity> SelectLocalContentList(string rows = "50", string startIndex = "0")
        {
            LocContentListDAL locContentListDAL = new LocContentListDAL();
            return locContentListDAL.SelectLocalContentList(rows, startIndex);
        }

        ///未测试
        /// <summary>
        /// 删除贴吧数据
        /// </summary>
        /// <param name="strid"></param>
        /// <returns>0传入数据为空，1插入成功，2部分数据插入失败 </returns>
        public int DeleteLocalContent(string[] strid)
        {
            LocContentListDAL locContentListDAL = new LocContentListDAL();
            return locContentListDAL.DeleteLocalContent(strid);
        }

        //修改好 未测试
        ///java  // 测试通过
        /// <summary>
        /// 插入贴吧标题 到本地数据
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="listBarNameEntity"></param>
        /// <returns></returns>
        public int InsertLocalContent(List<ContentEntity> listContentEntity)
        {
            LocContentListDAL locContentListDAL = new LocContentListDAL();
            return locContentListDAL.InsertLocalContent(listContentEntity);
        }

        //不使用
        /// <summary>
        /// 更新贴吧标题
        /// </summary>
        /// <param name="listResultEntity"></param>
        /// <returns></returns>
        public int UpdateLocalContent(List<ContentEntity> listResultEntity)
        {
            LocContentListDAL locContentListDAL = new LocContentListDAL();
            return locContentListDAL.UpdateLocalContent(listResultEntity);
        }

        //修改好未测试
        /// <summary>
        /// 查询总行数数据
        /// </summary>
        /// <returns></returns>
        public int SelectAllCount()
        {
            LocContentListDAL locContentListDAL = new LocContentListDAL();
            return locContentListDAL.SelectAllCount();
        }

        //修改好未测试
        /// <summary>
        /// 业务删除内容（更新）
        /// </summary>
        /// <param name="strid"></param>
        /// <returns>0 失败, 2部分错误 ，1完成 </returns>
        public int DelUpdateContente(string[] strid)
        {
            LocContentListDAL locContentListDAL = new LocContentListDAL();
            return locContentListDAL.DelUpdateContente(strid);
        }
    }
}
