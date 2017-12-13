using System;
using System.IO;

namespace Jieba.Net.Core.Config
{
    /// <summary>
    /// 默认配置
    /// </summary>
    public class DefaultConfig : Configuration
    {
        /// <summary>
        /// 分词器默认字典名称
        /// </summary>
        private readonly string mainDictFileName = "main.dict";

        /// <summary>
        /// 默认字典文件夹名称
        /// </summary>
        private readonly string dictDirctoryName = "dict";


        /// <summary>
        /// 基础路径
        /// </summary>

        private readonly string base_path = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 分词器默认字典路径
        /// </summary>
        public string MainDictFile { get; }

        /// <summary>
        /// 主词库路径
        /// </summary>

        public string MainDictionary { get; }


        public DefaultConfig() : this(null)
        {

        }

        public DefaultConfig(string directoryName)
        {
            dictDirctoryName = dictDirctoryName ?? directoryName;
            //词库文件夹
            MainDictionary = Path.Combine(base_path, dictDirctoryName);
            //主词典文件
            MainDictFile = Path.Combine(MainDictionary, mainDictFileName);
        }
    }
}
