using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OWZXEnum
{
    /// <summary>
    /// 机会状态
    /// </summary>
    public enum EnumOpportunityStatus
    {
        [DescriptionAttribute("全部")]
        All = -1,
        [DescriptionAttribute("正常")]
        New = 0,
        [DescriptionAttribute("正常")]
        Normal = 1,
        [DescriptionAttribute("转为订单")]
        Success = 2,
        [DescriptionAttribute("已关闭")]
        Close = 3,
        [DescriptionAttribute("已删除")]
        Delete = 9
    }
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum EnumOrderStatus
    {
        [DescriptionAttribute("全部")]
        All = -1,
        [DescriptionAttribute("草案订单")]
        Normal = 0,
        [DescriptionAttribute("待审核")]
        New = 1,
        [DescriptionAttribute("已审核")]
        Success = 2,
        [DescriptionAttribute("已退单")]
        Return = 3,
        [DescriptionAttribute("作废")]
        Invalid = 4,
        [DescriptionAttribute("已删除")]
        Delete = 9
    }

    /// <summary>
    /// 出库状态
    /// </summary>
    public enum EnumOutStatus
    {
        [DescriptionAttribute("全部")]
        All = -1,
        [DescriptionAttribute("待出库")]
        NoOut = 0,
        [DescriptionAttribute("已出库")]
        Out = 1
    }

    /// <summary>
    /// 发货状态
    /// </summary>
    public enum EnumSendStatus
    {
        [DescriptionAttribute("全部")]
        All = -1,
        [DescriptionAttribute("待出库")]
        NoOut = 0,
        [DescriptionAttribute("待发货")]
        Out = 1,
        [DescriptionAttribute("已发货")]
        Send = 2,
        [DescriptionAttribute("已签收")]
        Sign = 3,
        /// <summary>
        /// 查询用所有已出库
        /// </summary>
        AllOut = 11,
        /// <summary>
        /// 查询用所有已发货
        /// </summary>
        AllSend = 12,
    }
    /// <summary>
    /// 快递方式
    /// </summary>
    public enum EnumExpressType
    {
        [DescriptionAttribute("全部")]
        All = -1,
        [DescriptionAttribute("邮寄")]
        Post = 0,
        [DescriptionAttribute("海运")]
        Sea = 1,
        [DescriptionAttribute("空运")]
        Air = 2,
        [DescriptionAttribute("自提")]
        Self = 3
    }

    /// <summary>
    /// 后台订单状态
    /// </summary>
    public enum EnumClientOrderStatus
    {
        [DescriptionAttribute("全部")]
        All = -1,
        [DescriptionAttribute("未审核")]
        NoPay = 0,
        [DescriptionAttribute("已审核")]
        Pay = 1,
        [DescriptionAttribute("删除")]
        Delete = 9
    }
    /// <summary>
    /// 后台订单支付状态
    /// </summary>
    public enum EnumClientOrderPay
    {
        [DescriptionAttribute("全部")]
        All = -1,
        [DescriptionAttribute("未支付")]
        NoPay = 0,
        [DescriptionAttribute("已支付")]
        Pay = 1,
        [DescriptionAttribute("部分付款")]
        PartPay = 2,
        [DescriptionAttribute("部分退款")]
        PartReturn = 3,
        [DescriptionAttribute("全额退款")]
        Return = 4
    }
    public enum EnumReturnStatus
    {
        [DescriptionAttribute("全部")]
        All = -1,
        [DescriptionAttribute("未退货")]
        Normal = 0,
        [DescriptionAttribute("已申请")]
        Apply = 1,
        [DescriptionAttribute("部分退货")]
        PartReturn = 2,
        [DescriptionAttribute("已退单")]
        Return = 3,
        /// <summary>
        /// 全部退单用于查询
        /// </summary>
        AllReturn=11
    }
}
