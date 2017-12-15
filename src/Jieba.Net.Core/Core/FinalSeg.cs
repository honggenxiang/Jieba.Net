// ********************************************
// 作者：honggenxiang
// 时间：2017-12-15 16:25:32
// ********************************************


using System.Collections.Generic;

namespace Jieba.Net.Core.Core
{
    /// <summary>
    /// 1.观测序列；
    /// 2.隐藏状态序列；  
    /// 3.状态初始概率；
    /// 4.状态转移概率；
    /// 5.状态发射概率；
    /// </summary>
    public class FinalSeg
    {

        /// <summary>
        /// 隐藏状态
        /// </summary>
        private enum State
        {
            /// <summary>
            /// 首字
            /// </summary>
            B,
            /// <summary>
            /// 中间
            /// </summary>
            M,
            /// <summary>
            /// 结尾
            /// </summary>
            E,
            /// <summary>
            /// 单字
            /// </summary>
            S
        }

        /// <summary>
        /// 状态初始概率
        /// </summary>

        private static Dictionary<State, double> probStart = new Dictionary<State, double>
        {
            [State.B] = -0.26268660809250016,
            [State.E] = -3.14e+100,
            [State.M] = -3.14e+100,
            [State.S] = -1.4652633398537678
        };

        /// <summary>
        /// 状态转移概率
        /// </summary>

        private static Dictionary<State, Dictionary<State, double>> probTrans = new Dictionary<State, Dictionary<State, double>>
        {
            [State.B] =
            {
                [State.E] = -0.510825623765990,
                [State.M] = -0.916290731874155,
            },
            [State.E] =
            {
                [State.B] = -0.5897149736854513,
                [State.S] = -0.8085250474669937
            },
            [State.M] =
            {
                [State.E] = -0.33344856811948514,
                [State.M] = -1.2603623820268226
            },
            [State.S] =
            {
                [State.B] = -0.26268660809250016,
                [State.S] = -0.26268660809250016
            },
        };
    }
}