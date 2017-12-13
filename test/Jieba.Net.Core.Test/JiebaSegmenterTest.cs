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
    }
}
