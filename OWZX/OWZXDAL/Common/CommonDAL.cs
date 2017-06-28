using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace OWZXDAL
{
    public class CommonDAL : BaseDAL
    {
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMenus()
        {
            DataTable dt = GetDataTable("select * from owzx_Menu where Type=1 and IsHide=0 order by Sort ");
            return dt;
        }
        /// <summary>
        /// 获取后台菜单
        /// </summary>
        /// <returns></returns>
        public DataTable GetManageMenus()
        {
            DataTable dt = GetDataTable("select * from owzx_Menu where Type=2 and IsHide=0 order by Sort ");
            return dt;
        }
 

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">字段名</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static object Select(string tableName, string columnName, string where)
        {
            StringBuilder build = new StringBuilder();
            build.Append("select " + columnName + " from " + tableName);
            if (!string.IsNullOrEmpty(where))
            {
                build.Append(" where " + where);
            }
            return ExecuteScalar(build.ToString());
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">字段名</param>
        /// <param name="columnValue">字段值</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static int Update(string tableName, string columnName, string columnValue, string where)
        {
            StringBuilder build = new StringBuilder();
            build.Append("Update " + tableName + " set " + columnName + " = @columnValue ");
            if (!string.IsNullOrEmpty(where))
            {
                build.Append(" where " + where);
            }
            SqlParameter[] paras = { 
                                    new SqlParameter("@columnValue",columnValue)
                                   };
            return ExecuteNonQuery(build.ToString(), paras, CommandType.Text);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static int Delete(string tableName, string where)
        {
            StringBuilder build = new StringBuilder();
            build.Append("Delete from " + tableName);
            if (!string.IsNullOrEmpty(where))
            {
                build.Append(" where " + where);
            }
            return ExecuteNonQuery(build.ToString());
        }


        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">列明</param>
        /// <param name="condition">条件</param>
        /// <param name="key">主键，分页条件</param>
        /// <param name="orderColumn">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="totalNum">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="isAsc">0主键降序 1主键升序</param>
        /// <returns></returns>
        public static DataTable GetPagerData(string tableName, string columns, string condition, string key, string orderColumn, int pageSize, int pageIndex, out int totalNum, out int pageCount, int isAsc)
        {
            string procName = "P_GetPagerData";
            SqlParameter[] paras = { 
                                        new SqlParameter("@tableName",DbType.String),
                                        new SqlParameter("@columns",DbType.String),
                                        new SqlParameter("@condition",DbType.String),
                                        new SqlParameter("@key",DbType.String),
                                        new SqlParameter("@orderColumn",DbType.String),
                                        new SqlParameter("@pageSize",DbType.Int32),
                                        new SqlParameter("@pageIndex",DbType.Int32),
                                        new SqlParameter("@totalCount",DbType.Int32),
                                        new SqlParameter("@pageCount",DbType.Int32),
                                        new SqlParameter("@isAsc",DbType.Int32),
                                   };
            paras[0].Value = tableName;
            paras[1].Value = columns;
            paras[2].Value = condition;
            paras[3].Value = key;
            paras[4].Value = orderColumn;
            paras[5].Value = pageSize;
            paras[6].Value = pageIndex;
            paras[7].Direction = ParameterDirection.Output;
            paras[8].Direction = ParameterDirection.Output;
            paras[9].Value = isAsc;

            DataTable dt = GetDataTable(procName, paras, CommandType.StoredProcedure);
            totalNum = Convert.ToInt32(paras[7].Value);
            pageCount = Convert.ToInt32(paras[8].Value);
            return dt;
        }

        public static DataSet GetReplysByType(string guid, int type, string agentid, int pageSize, int pageIndex,
            ref int totalCount, ref int pageCount)
        {
            SqlParameter[] paras = { 
                                       new SqlParameter("@totalCount",SqlDbType.Int),
                                       new SqlParameter("@pageCount",SqlDbType.Int),
                                       new SqlParameter("@pageSize",pageSize),
                                       new SqlParameter("@pageIndex",pageIndex),
                                       new SqlParameter("@ID",guid),
                                       new SqlParameter("@Type",type),
                                       new SqlParameter("@Agentid",agentid), 
   
                                   };
            paras[0].Value = totalCount;
            paras[1].Value = pageCount;

            paras[0].Direction = ParameterDirection.InputOutput;
            paras[1].Direction = ParameterDirection.InputOutput;
            DataSet ds = GetDataSet("P_GetReplysByType", paras, CommandType.StoredProcedure, "Replys|Attachments");
            totalCount = Convert.ToInt32(paras[0].Value);
            pageCount = Convert.ToInt32(paras[1].Value);
            return ds; 
        }
        public static bool AddReplyAttachments(string replyid, int attachmentType,
            string serverUrl, string filePath, string fileName, string originalName, string thumbnailName, long size,
            string userid, string agentid, string clientid, SqlTransaction tran)
        {
            SqlParameter[] paras = { 
                                        new SqlParameter("@ReplyID",replyid),
                                        new SqlParameter("@Type",attachmentType),
                                        new SqlParameter("@ServerUrl",serverUrl),
                                        new SqlParameter("@FilePath",filePath),
                                        new SqlParameter("@FileName",fileName),
                                        new SqlParameter("@OriginalName",originalName),
                                        new SqlParameter("@ThumbnailName",thumbnailName),
                                        new SqlParameter("@Size",size),
                                        new SqlParameter("@UserID",userid),
                                        new SqlParameter("@ClientID",clientid),
                                        new SqlParameter("@Agentid",agentid)
                                   };
            return ExecuteNonQuery(tran, "P_AddReplyAttachment", paras, CommandType.StoredProcedure) > 0; 
        }

    }
}
