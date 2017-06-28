using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OWZXEnum
{
    /// <summary>
    /// 活动阶段
    /// </summary>
    public enum EnumActivityStage
    {
        All = -1,
        /// <summary>
        /// 已结束
        /// </summary>
        End = 1,
        /// <summary>
        /// 进行中
        /// </summary>
        Runing = 2,
        /// <summary>
        /// 未开始
        /// </summary>
        NoBegin = 3
    }

    /// <summary>
    /// 客户阶段标记
    /// </summary>
    public enum EnumCustomStageMark
    {
        [DescriptionAttribute("普通阶段")]
        Normal = 0,
        [DescriptionAttribute("新客户阶段")]
        New = 1,
        [DescriptionAttribute("成交阶段")]
        Success = 2
    }

    /// <summary>
    /// 客户阶段
    /// </summary>
    public enum EnumCustomStageStatus
    {
        [DescriptionAttribute("新客户")]
        New = 1,
        [DescriptionAttribute("机会客户")]
        Opportunity = 2,
        [DescriptionAttribute("成交客户")]
        Success = 3
    }

    /// <summary>
    /// 客户状态
    /// </summary>
    public enum EnumCustomStatus
    {
        All = -1,
        [DescriptionAttribute("正常")]
        Normal = 1,
        [DescriptionAttribute("关闭")]
        Close = 2,
        [DescriptionAttribute("丢失")]
        Loses = 3,
        [DescriptionAttribute("删除")]
        Delete = 9
    }
    /// <summary>
    /// 客户规模
    /// </summary>
    public enum EnumCustomerExtend
    {
        /// <summary>
        /// 一个
        /// </summary>
        [DescriptionAttribute("")]
        OnePeople = 0,
        /// <summary>
        /// 很少
        /// </summary>
        [DescriptionAttribute("0-49")]
        Small = 1,
        /// <summary>
        /// 少量
        /// </summary>
        [DescriptionAttribute("50-99")]
        Little = 2,
        /// <summary>
        /// 一般
        /// </summary>
        [DescriptionAttribute("100-199")]
        SameAs = 3,
        /// <summary>
        /// 有点多
        /// </summary>
        [DescriptionAttribute("200-499")]
        Many = 4,
        /// <summary>
        /// 很多
        /// </summary>
        [DescriptionAttribute("500-999")]
        Multi = 5,
        /// <summary>
        /// 巨大
        /// </summary>
        [DescriptionAttribute("1000+")]
        Huge = 6
    }
}
