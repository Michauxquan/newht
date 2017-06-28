using OWZXDAL;
using OWZXDAL.Manage;
using OWZXEntity;
using OWZXEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OWZXBusiness
{

    public class CommonBusiness
    {
        #region Cache
        private static List<Menu> _clientMenus;
         
        /// <summary>
        /// 客户端菜单
        /// </summary>
        public static List<Menu> ClientMenus
        {
            get
            {
                if (_clientMenus == null)
                {
                    _clientMenus = new List<Menu>();
                    DataTable dt = new CommonDAL().GetMenus();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Menu model = new Menu();
                        model.FillData(dr);
                        _clientMenus.Add(model);
                    }

                    foreach (var menu in _clientMenus)
                    {
                        menu.ChildMenus = _clientMenus.Where(m => m.PCode == menu.MenuCode).ToList();
                    }

                }
                return _clientMenus;
            }
            set
            {
                _clientMenus = value;
            }
        } 
        private static List<Menu> _manageMenus;
        /// <summary>
        /// 后台端菜单
        /// </summary>
        public static List<Menu> ManageMenus
        {
            get
            {
                if (_manageMenus == null)
                {
                    _manageMenus = new List<Menu>();
                    DataTable dt = new CommonDAL().GetManageMenus();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Menu model = new Menu();
                        model.FillData(dr);
                        _manageMenus.Add(model);
                    }

                    foreach (var menu in _manageMenus)
                    {
                        menu.ChildMenus = _manageMenus.Where(m => m.PCode == menu.MenuCode).ToList();
                    }

                }
                return _manageMenus;
            }
            set
            {
                _manageMenus = value;
            }
        }
        #endregion
         
        /// <summary>
        /// 修改表中某字段值
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">字段名</param>
        /// <param name="columnValue">字段值</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static bool Update(string tableName, string columnName, object columnValue, string where)
        {
            int obj = CommonDAL.Update(tableName, columnName, columnValue.ToString(), where);
            return obj > 0;
        }

        /// <summary>
        /// 获取表中某字段值
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">字段名</param>
        /// <param name="columnValue">字段值</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static object Select(string tableName, string columnName, string where)
        {
            object obj = CommonDAL.Select(tableName, columnName, where);
            return obj;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static bool Delete(string tableName, string where)
        {
            int obj = CommonDAL.Delete(tableName, where);
            return obj > 0;
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
        /// <param name="pageCount">当前页数</param>
        /// <param name="totalNum">总记录数</param>
        /// <param name="totalPage">总页数</param>
        /// <param name="isAsc">主键是否升序</param>
        /// <returns></returns>
        public static DataTable GetPagerData(string tableName, string columns, string condition, string key, string orderColumn, int pageSize, int pageIndex, out int totalNum, out int pageCount, bool isAsc)
        {
            int asc = 0;
            if (isAsc)
            {
                asc = 1;
            }
            return CommonDAL.GetPagerData(tableName, columns, condition, key, orderColumn, pageSize, pageIndex, out totalNum, out pageCount, asc);
        }
        
        /// <summary>
        /// 获取分页数据集合(默认降序)
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">列明</param>
        /// <param name="condition">条件</param>
        /// <param name="key">主键，分页条件</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageCount">当前页数</param>
        /// <param name="totalNum">总记录数</param>
        /// <param name="totalPage">总页数</param>
        /// <returns></returns>
        public static DataTable GetPagerData(string tableName, string columns, string condition, string key, int pageSize, int pageIndex, out int totalNum, out int pageCount)
        {
            return CommonDAL.GetPagerData(tableName, columns, condition, key, "", pageSize, pageIndex, out totalNum, out pageCount, 0);
        }
        
        /// <summary>
        /// 获取分页数据集合(默认降序)
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">列明</param>
        /// <param name="condition">条件</param>
        /// <param name="key">主键，分页条件</param>
        /// <param name="orderColumn">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageCount">当前页数</param>
        /// <param name="totalNum">总记录数</param>
        /// <param name="totalPage">总页数</param>
        /// <returns></returns>
        public static DataTable GetPagerData(string tableName, string columns, string condition, string key, string orderColumn, int pageSize, int pageIndex, out int totalNum, out int pageCount)
        {
            return CommonDAL.GetPagerData(tableName, columns, condition, key, orderColumn, pageSize, pageIndex, out totalNum, out pageCount, 0);
        }

        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">列明</param>
        /// <param name="condition">条件</param>
        /// <param name="key">主键，分页条件</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageCount">当前页数</param>
        /// <param name="totalNum">总记录数</param>
        /// <param name="totalPage">总页数</param>
        /// <param name="isAsc">主键是否升序</param>
        /// <returns></returns>
        public static DataTable GetPagerData(string tableName, string columns, string condition, string key, int pageSize, int pageIndex, out int totalNum, out int pageCount, bool isAsc)
        {
            int asc = 0;
            if (isAsc)
            {
                asc = 1;
            }
            return CommonDAL.GetPagerData(tableName, columns, condition, key, "", pageSize, pageIndex, out totalNum, out pageCount, asc);
        }

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Enumtype"></param>
        /// <returns></returns>
        public static string GetEnumDesc<T>(T Enumtype)
        {
            if (Enumtype == null) throw new ArgumentNullException("Enumtype");
            if (!Enumtype.GetType().IsEnum) throw new Exception("参数类型不正确");
            return ((DescriptionAttribute)Enumtype.GetType().GetField(Enumtype.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;
        }

        public static int GetEnumindexByDesc<T>(T Enumtype, string enumDesc)
        {
            int result  = 0;
            if (Enumtype == null) throw new ArgumentNullException("Enumtype");
            if (!Enumtype.GetType().IsEnum) throw new Exception("参数类型不正确");
            Array arrays = Enum.GetValues(typeof (T));
            for (int i = 0; i < arrays.LongLength; i++)
            {
                T test = (T) arrays.GetValue(i);
                FieldInfo fieldInfo = test.GetType().GetField(test.ToString());
                object[] attribArray = fieldInfo.GetCustomAttributes(false);
                DescriptionAttribute des = (DescriptionAttribute) attribArray[0];
                if (des.Description == enumDesc)
                {
                    result= i;
                    break;
                }
            }
            return result; 
        }

        /// <summary>
        /// Excel下拉框数据源 Michuax
        /// </summary>
        /// <param name="dropsource">Enum:DropSourceList</param>
        /// <returns></returns>
        public static string GetDropList(DropSourceList dropsource,string clientID="")
        {
            string listStr ="";
            switch (dropsource)
            { 
                default:
                     listStr="";
                    break;;
            }
            return listStr;
        }

        /// <summary>
        /// DataRow 转实体类 Michaux 添加
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T FillModel<T>(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }
            T model = (T)Activator.CreateInstance(typeof(T));

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance;
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName, flag);
                if (propertyInfo != null && dr[i] != DBNull.Value)
                    propertyInfo.SetValue(model, dr[i], null);
                else continue;
            }
             
            return model;
        }

        public static object getClientSetting(EnumSettingKey key, string columnName ,string agentid, string clientid)
        {
            return Select("ClientSetting", columnName, "ClientID='" + clientid + "' and [KeyType]=" + (int)key);
        }

        /// <summary>
        /// 设置系统参数
        /// </summary>
        /// <returns></returns>
        public static bool SetClientSetting(EnumSettingKey key, object value, string userid, string ip, string agentid, string clientid)
        {
            bool bl = Update("ClientSetting", GetEnumDesc<EnumSettingKey>(key), value, "ClientID='" + clientid + "' and [KeyType]=" + (int)key);
            if (bl)
            {
                string message = "";
                switch (key)
                {
                    case EnumSettingKey.IntegralSource:
                        message = Convert.ToInt32(value) == 1 ? "积分规则变更为以产品积分计算" : "积分规则变更为以订单金额计算";
                        break;
                    case EnumSettingKey.IntegralScale:
                        message = "金额积分比例设置为：" + value;
                        break;
                }
            }
            return bl;
        }

        // <summary>
        /// 添加服务日志
        /// </summary>
        public static void WriteLog(string str, int logType=1,string logname="log")
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd");
            string fileExtention = ".txt";
            string directoryName = logname;
            string infohead = "[Info] ";
            FileStream fs = null;
            if (logType == 2)
            {
                infohead = "[Error] ";
            }
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log\\" + directoryName;
            infohead += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            fs = new FileStream(path + "\\" + fileName + fileExtention, FileMode.OpenOrCreate,
                FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(infohead + str + "\n");

            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
