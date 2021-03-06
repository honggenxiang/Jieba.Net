﻿using System.Linq;
using Jieba.Net.Core.Config;
using Jieba.Net.Core.Core;
using Jieba.Net.Core.Dict;
using Xunit;

namespace Jieba.Net.Core.Test
{
    public class JiebaSegmenterTest
    {
        public JiebaSegmenterTest()
        {
            Dictionary.Initial(new DefaultConfig());
        }

        [Fact]
        public void DAG()
        {

            var dict = new JiebaSegmenter().CreateDAG("洪根祥");
        }

        [Fact]
        public void Calc()
        {
            //var @string = "支持和Razor视图引擎捆绑反编译器，增强导航，重新设计的设置管理，不同语言的新的代码检查功能";
            var @string = "陈琛爱洪根祥";

            var jiebaSegmenter = new JiebaSegmenter();
            var tokens = jiebaSegmenter.Calc(@string, jiebaSegmenter.CreateDAG(@string));
        }

        [Fact]
        public void SentenceProcess()
        {
            //var @string = "陈琛爱洪根祥";
            var @string = "我来到北京清华大学我爱北京天安门用全面的测试工具编写高质量代码";
            var tokens = new JiebaSegmenter().SentenceProcess(@string);
        }

        [Fact]
        public void Viterbi()
        {
            var rs =  FinalSeg.Output("我爱北京天安门").ToList();
        }
    }
}
