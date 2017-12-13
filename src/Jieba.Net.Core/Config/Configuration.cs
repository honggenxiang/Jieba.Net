namespace Jieba.Net.Core.Config
{
    /// <summary>
    /// 配置管理类接口
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public interface Configuration
    {
        /// <summary>
        /// 获取主词典路径
        /// </summary>
        string MainDictFile { get; }

        /// <summary>
        /// 获取词典文件夹
        /// </summary>
        string MainDictionary { get; }
    }
}
