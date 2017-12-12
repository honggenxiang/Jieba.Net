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
        /// UseSmart标志位
        /// UseSmart=true，分词器使用智能切分策略,=false则使用细粒度切分
        /// </summary>
        bool UseSmart { get; set; }
        /// <summary>
        /// 获取主词典路径
        /// </summary>
        string MainDictionary { get; }
    }
}
