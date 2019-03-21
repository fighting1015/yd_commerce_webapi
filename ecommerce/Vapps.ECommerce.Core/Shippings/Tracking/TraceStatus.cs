using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Shippings.Tracking
{
    public enum TraceStatus
    {
        /// <summary>
        /// 无轨迹
        /// </summary>
        NoTrace = 0,

        /// <summary>
        /// 已揽收
        /// </summary>
        Taked = 1,

        /// <summary>
        /// 在途
        /// </summary>
        OnPassag = 2,

        /// <summary>
        /// 到达派件城市
        /// </summary>
        DestinationCity = 201,

        /// <summary>
        /// 派件中
        /// </summary>
        Delivering = 202,

        /// <summary>
        /// 已签收
        /// </summary>
        InBox = 211,

        /// <summary>
        /// 已签收
        /// </summary>
        Received = 3,

        /// <summary>
        /// 已取出快递柜或驿站
        /// </summary>
        TackedFormBox = 311,

        /// <summary>
        /// 问题件
        /// </summary>
        Issue = 4,

        /// <summary>
        /// 问题件
        /// </summary>
        NoTraceInfo = 401,

        /// <summary>
        /// 拒收(退件)
        /// </summary>
        RejectByTimeOut = 402,

        /// <summary>
        /// 拒收(退件)
        /// </summary>
        TimeOut2Update = 403,

        /// <summary>
        /// 拒收(退件)
        /// </summary>
        IssueWithReject = 404,

        /// <summary>
        /// 快递柜或驿站超时未取
        /// </summary>
        TimeOutInBox = 412,
    }
}
