using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OWZXDAL;
using System.Threading.Tasks;
using OWZXEnum;
using OWZXEntity;
using System.Data;
using OWZXEntity.Manage;

namespace OWZXBusiness
{
    public class LogBusiness
    {
        public static LogBusiness BaseBusiness = new LogBusiness();

        public static List<UsersLog> UsersLogList( int pageSize, int pageIndex, ref int totalCount, ref int pageCount,string keyWords, int uid = -1)
        {
            string whereSql = " 1=1 ";
            if (uid > -1)
            {
                whereSql += " and a.uid=" + uid + " ";
            } 
            if (!string.IsNullOrEmpty(keyWords))
            {
                whereSql += " and (a.UserName like '%" + keyWords + "%' or a.mobile like'%" + keyWords + "%' or a.email like'%" + keyWords + "%') ";
            }
            string cstr = @" a.*,b.Email,b.UserName,b.mobile,b.nickname";
            DataTable dt = CommonBusiness.GetPagerData("Owzx_UsersLog a left join owzx_users b on a.uid=b.uid  ", cstr, whereSql, "a.Uid", pageSize, pageIndex, out totalCount, out pageCount);
            List<UsersLog> list = new List<UsersLog>(); 
            foreach (DataRow item in dt.Rows)
            {
                UsersLog model = new UsersLog();
                model.FillData(item); 
                list.Add(model);
            }

            return list;
        } 

        #region 添加

        public static async void AddLoginLog(int uid, int type, string operateip, string ipName, string remark)
        {
            await LogDAL.AddLoginLog(uid,type,operateip,ipName,remark);
        } 
        #endregion
    }
}
