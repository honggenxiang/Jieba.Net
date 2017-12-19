// ********************************************
// 作者：honggenxiang
// 时间：2017-12-19 11:19:57
// ********************************************

using System.Collections.Generic;
using Xunit;

namespace Jieba.Net.Core.Test
{
    public class FinalSegTest
    {
        [Fact]
        public void StateTest()
        {
            var probTrans = new Dictionary<State, Dictionary<State, double>>
            {
                [State.B] = new Dictionary<State, double>
                {
                    [State.E] = -0.510825623765990,
                    [State.M] = -0.916290731874155,
                },
                [State.E] = new Dictionary<State, double>
                {
                    [State.B] = -0.5897149736854513,
                    [State.S] = -0.8085250474669937
                },
                [State.M] = new Dictionary<State, double>
                {
                    [State.E] = -0.33344856811948514,
                    [State.M] = -1.2603623820268226
                },
                [State.S] = new Dictionary<State, double>
                {
                    [State.B] = -0.26268660809250016,
                    [State.S] = -0.26268660809250016
                },
            };
        }
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
    }
}