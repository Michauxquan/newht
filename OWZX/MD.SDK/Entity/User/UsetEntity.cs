using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.SDK
{
    public class UsetEntity
    {
        public string id
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        public string avatar
        {
            get;
            set;
        }

        public string email
        {
            get;
            set;
        }

        public string mobile_phone
        {
            get;
            set;
        }

        public string work_phone
        {
            get;
            set;
        }

        public string token { get; set; }

        public string firstname
        {
            get
            {
                return Net.Sourceforge.Pinyin4j.PinyinHelper.ToHanyuPinyinString(this.name, new Net.Sourceforge.Pinyin4j.Format.HanyuPinyinOutputFormat(), " ").ToCharArray()[0].ToString().ToUpper();
            }
        }

        public ProjectEntity project
        {
            get;
            set;
        }
    }
}
