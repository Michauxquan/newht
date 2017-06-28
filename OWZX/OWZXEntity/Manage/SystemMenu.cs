using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OWZXEntity.Manage
{
    [Serializable]
    public partial class SystemMenu
    {
        public SystemMenu()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int AutoID
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string MenuCode
        {
            set;
            get;
        }

        public string Name
        {
            set;
            get;
        }

        public string Area
        {
            set;
            get;
        }

        public string Controller
        {
            set;
            get;
        }

        public string View
        {
            set;
            get;
        }

        public string IcoPath
        {
            set;
            get;
        }

        public string IcoHover
        {
            set;
            get;
        }

        public int Type
        {
            set;
            get;
        }

        public int IsHide
        {
            set;
            get;
        }

        public string PCode
        {
            set;
            get;
        }

        public string PCodeName
        {
            set;
            get;
        }

        public int Sort
        {
            set;
            get;
        }

        public int IsMenu
        {
            set;
            get;
        }

        public int Layer
        {
            set;
            get;
        }

        public int IsLimit
        {
            set;
            get;
        }
        #endregion Model

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dr"></param>
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
