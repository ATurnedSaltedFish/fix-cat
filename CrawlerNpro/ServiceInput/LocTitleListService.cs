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
        /// <summary>
        ///查询数据返回datatable
        /// </summary>
        /// <param name="row">返回数据数目 默认值50</param>
        /// <param name="startIndex">开始行号 默认值0</param>
        /// <returns></returns>
        public List<ResultEntity> SelectLocalTitleList(string row, string startIndex)
        {
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            var list = locTitleListDAL.SelectLocalTitleList(row, startIndex);
            if (list.Count < 0)
            {
                return null;
            }
            return list;
        }

        /// <summary>
        /// 查询本地贴吧标题
        /// </summary>
        /// <param name="row"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public DataTable SelectLocalTitleDataTable(string row = "50", string startIndex = "0")
        {
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            var dataTable = locTitleListDAL.SelectLocalTBTitle(row, startIndex);
            if (dataTable == null)
            {
                return null;
            }
            return dataTable;
        }

        /// <summary>
        /// 查询贴吧标题 Icode是否存在 
        /// </summary>
        /// <param name="resultEntity"></param>
        /// <returns></returns>
        public bool SelectCreateCode(ResultEntity resultEntity)
        {
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            return locTitleListDAL.SelectCreateCode(resultEntity);
        }

        /// <summary>
        /// 将贴吧标题List插入到本地数据库
        /// </summary>
        /// <param name="listTitleContentEntity">标题</param>
        /// <returns>小于0 失败</returns>
        public int InsertLocalTBTitle(List<ResultEntity> listResultEntity)
        {
            if (listResultEntity.Count <= 0)
            {
                return 0;
            }
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            return locTitleListDAL.InsertLocalTBTitle(listResultEntity);
        }

        /// <summary>
        /// 将贴吧标题插入到本地数据库
        /// </summary>
        /// <param name="listTitleContentEntity">标题</param>
        /// <returns>小于0 失败</returns>
        public int InsertLocalTBTitle(ResultEntity resultEntity)
        {
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            return locTitleListDAL.InsertLocalTBTitle(resultEntity);
        }

        public int InsertLocalTBTitleEx(List<ResultEntity> listResultEntity)
        {
            List<ResultEntity> listResultEntityServ= new List<ResultEntity>();
            ResultEntity resultEntityServ = new ResultEntity();
            if (listResultEntity.Count <= 0)
            {
                return 0;
            }
            foreach (var item in listResultEntity)
            {
               var result=SelectCreateCode(item);
                if (result)//存在执行update
                {
                      UpdateLocalTBTitle(item);
                    ///返回值修改
                }
                else //执行insert
                {
                      InsertLocalTBTitle(item);
                    ///返回值修改
                }
            }
            return 0;
        }

        /// <summary>
        /// 更新sqlite贴吧标题List
        /// </summary>
        /// <param name="listResultEntity"></param>
        /// <returns></returns>
      public int UpdateLocalTBTitle(List<ResultEntity> listResultEntity)
        {
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            var result= locTitleListDAL.UpdateLocalTBTitle(listResultEntity);
            return result;
        }

        /// <summary>
        /// 更新sqlite贴吧标题
        /// </summary>
        /// <param name="resultEntity"></param>
        /// <returns></returns>
        public int UpdateLocalTBTitle(ResultEntity resultEntity)
        {
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            var result = locTitleListDAL.UpdateLocalTBTitle(resultEntity);
            return result;
        }

        /// <summary>
        /// 删除id
        /// </summary>
        /// <param name="strid"></param>
        /// <returns></returns>
        public bool DeleteLocalTBTItle(string[] strid)
        {
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            var result = locTitleListDAL.DeleteLocalTBTitle(strid);
            if (result == 1)
            {
                return false;
            }
            else if (result == 0)
            {
                return false;
            }
            else if (result == 2)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查询总行数
        /// </summary>
        /// <returns></returns>
        public int SelectAllCount()
        {
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            return locTitleListDAL.SelectAllCount();
        }
        /// <summary>
        ///业务删除贴吧标题
        /// </summary>
        /// <param name="strid"></param>
        /// <returns></returns>
        public int DelUpdateTBTitl(string[] strid)
        {
            LocTitleListDAL locTitleListDAL = new LocTitleListDAL();
            return locTitleListDAL.DelUpdateTBTitle(strid);
        }
    }
}
