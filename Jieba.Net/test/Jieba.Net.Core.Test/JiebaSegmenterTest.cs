using System;
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
            Dictionary.Initial(DefaultConfig.GetInstance());
        }

        [Fact]
        public void DAG(){
            new JiebaSegmenter().CreateDAG("地质矿产部");
        }
    }
}
