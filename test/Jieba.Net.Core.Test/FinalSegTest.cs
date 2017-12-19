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
            var x1 = -3.14e+100;
            var x2 = -3.14e100;
            Assert.Equal(x1,x2);
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