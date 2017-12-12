using System;
using System.IO;

namespace Jieba.Net.Core.Config
{
    /// <summary>
    /// 默认配置
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class DefaultConfig : Configuration
    {
        /// <summary>
        /// 分词器默认字典路径
        /// </summary>
        private const string MainDictFile = "main.dict";
        private const string DictDirctory = "dict";
        /// <summary>
        /// 基础路径
        /// </summary>

        private readonly string base_path = AppDomain.CurrentDomain.BaseDirectory;

        private DefaultConfig()
        {
            //IK词库部分
            MainDictionary = Path.Combine(base_path,DictDirctory, MainDictFile);         
        }
        /// <summary>
        /// 返回配置实例
        /// </summary>
        /// <returns></returns>

        public static Configuration GetInstance()
        {
            return new DefaultConfig();
        }
        /// <summary>
        /// 主词库路径
        /// </summary>

        public string MainDictionary
        {
            get; 

        }
        /// <summary>
        /// 是否智能分词
        /// </summary>
        public bool UseSmart
        {
            get; set;
        }
      
    }
}
