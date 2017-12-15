using System;

namespace Jieba.Net.Core.Core
{
    /// <summary>
    /// 字符类型
    /// </summary>
    [Flags]
    public enum CharType
    {
        /// <summary>
        /// 未识别字符
        /// </summary>
        CHAR_USELESS = 0x0000,
        /// <summary>
        /// 阿拉伯数字
        /// </summary>
        CHAR_ARABIC = 0x0001,
        /// <summary>
        /// 英文
        /// </summary>
        CHAR_ENGLISH = 0x0002,
        /// <summary>
        /// 中文
        /// </summary>
        CHAR_CHINESE = 0x0004,
        /// <summary>
        /// 其他日中韩字符
        /// </summary>
        CHAR_OTHER_CJK = 0x0008
    }
}
