using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZXDAL
{
    public class LogDAL :BaseDAL
    {
 

        public static Task<bool> AddLoginLog(int uid, int type, string operateip, string ipName, string remark)
        {  
            string commandText = string.Format("INSERT INTO [{0}userslog]([uid],[createtime],[remark],[type],[ip] ,[ipname]) VALUES({1},'{2}','{3}',{4},'{5}','{6}')",
                                           "owzx_",uid,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"),remark,type,operateip,ipName);
             
    
            return Task.Run(() => { return ExecuteNonQuery(commandText, null, CommandType.Text) > 0; });
        }
 
    }
}
