using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OWZXEntity.Manage
{
    [Serializable]
    public partial class UsersLog
    {
        public UsersLog()
        {
        }

        #region Model

        private int _autoid;
        private int _uid;
        private string _nickname;
        private string _username;
        private string _email;
        private string _mobile;
        private string _ipname;
        private string _ip;
        private int? _type = 0;
        private string _remark = "";
        private DateTime? _createtime = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }

        /// <summary>
        /// 
        /// </summary> 
        public int Uid
        {
            set { _uid = value; }
            get { return _uid; }
        }
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        public string NickName
        {
            set { _nickname = value; }
            get { return _nickname; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string IPName
        {
            set { _ipname = value; }
            get { return _ipname; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? Type
        {
            set { _type = value; }
            get { return _type; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Reamrk
        {
            set { _remark = value; }
            get { return _remark; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        #endregion
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }

    }


}

