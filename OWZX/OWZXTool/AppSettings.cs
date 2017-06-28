﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OWZXTool
{

    public class AppSettings
    {
        public static Settings Settings 
        {
            get 
            {               
                return new Settings();
            }
        }
    }

    public class Settings
    {
        /// <summary>
        /// 获取网站AppSettings配置信息
        /// </summary>
        /// <param name="web"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                string value = System.Configuration.ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                else
                {
                    return value;

                }
            }
        }
    }
}
