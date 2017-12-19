using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Jieba.Net.Core.Config;
using Jieba.Net.Core.Core;

namespace Jieba.Net.Core.Dict
{
    /// <summary>
    /// 词典管理,单例模式
    /// </summary>
    public class Dictionary
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object objLock = new object();
        /// <summary>
        /// 词典单例实例
        /// </summary>
        private static Dictionary singleton;
        /// <summary>
        /// 主词典对象
        /// </summary>
        private DictSegment mainDict;

        /// <summary>
        /// 配置对象
        /// </summary>
        private readonly Configuration cfg;

        private Dictionary(Configuration cfg)
        {
            this.cfg = cfg;
            LoadMainDict();
        }

        /// <summary>
        /// 词典初始化
        /// 由于Analyzer的词典采用Dictionary类的静态方法进行词典初始化
        /// 只有当Dictionary类被实际调用时，才会开始载入词典，
        /// 这将延长首次分词操作的时间
        /// 该方法提供了一个在应用加载阶段就初始化字典的手段
        /// </summary>
        /// <param name="cfg">配置</param>
        /// <returns>字典</returns>
        public static Dictionary Initial(Configuration cfg)
        {
            //双检锁
            if (singleton == null)
            {
                lock (objLock)
                {
                    if (singleton == null)
                    {
                        var dictionary = new Dictionary(cfg);
                        Interlocked.CompareExchange(ref singleton, dictionary, null);
                    }
                    else
                    {
                        throw new InvalidOperationException("Dictionary.Initial()不能被重复调用");

                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Dictionary.Initial()不能被重复调用");
            }
            return singleton;
        }
        /// <summary>
        /// 批量加载新词条
        /// </summary>
        /// <param name="words"></param>
        public void AddWords(List<KeyValuePair<string, WordNode>> words)
        {
            if (words == null) return;
            foreach (var word in words)
            {
                if (!string.IsNullOrEmpty(word.Key))
                {
                    //批量加载词条到主内存词典中
                    GetSingleton().mainDict.FillSegment(word.Key.Trim().ToLower().ToCharArray(), word.Value);
                }
            }
        }

        /// <summary>
        /// 批量移除(频闭)词条
        /// </summary>
        /// <param name="words"></param>

        public void DisableWords(List<string> words)
        {
            if (words != null)
            {
                foreach (var word in words)
                {
                    if (word != null)
                    {
                        //批量屏蔽词条
                        GetSingleton().mainDict.DisableSegment(word.Trim().ToLower().ToCharArray());
                    }
                }
            }
        }
        /// <summary>
        /// 获取词典单例
        /// </summary>
        /// <returns></returns>
        public static Dictionary GetSingleton()
        {
            if (singleton == null)
            {
                throw new InvalidOperationException("词典尚未初始化，请先调用Initial方法");
            }
            return singleton;
        }
        /// <summary>
        /// 检索匹配主词典
        /// </summary>
        /// <param name="charArray"></param>
        /// <returns></returns>
        public Hit MatchInMainDict(char[] charArray)
        {
            return GetSingleton().mainDict.Match(charArray);
        }
        /// <summary>
        /// 检索匹配主词典
        /// </summary>
        /// <param name="charArray"></param>
        /// <param name="begin"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public Hit MatchInMainDict(char[] charArray, int begin, int length)
        {
            return GetSingleton().mainDict.Match(charArray, begin, length);
        }


        /// <summary>
        /// 从匹配的Hit中取出DictSegment，继续向下匹配
        /// </summary>
        /// <returns></returns>
        public Hit MatchWithHit(char[] charArray, int currentIndex, Hit matchedHit)

        {
            var ds = matchedHit.MatchedDictSegment;
            return ds.Match(charArray, currentIndex, 1, matchedHit);
        }


        /// <summary>
        /// 加载主词典及扩展词典
        /// </summary>
        private void LoadMainDict()
        {
            var files = Directory.GetFiles(cfg.MainDictionary, "*.dict");
            //读取量词词典文件
            if (!files.Contains(cfg.MainDictFile))
            {
                throw new InvalidOperationException("未发现主词库词典!!!");
            }
            //建立一个主词典实例
            mainDict = new DictSegment((char)0);
            foreach (var file in files)
            {
                var theWord = File.ReadAllLines(file, Encoding.UTF8);
                foreach (var word in theWord)
                {
                    if (IsValidWord(word))
                    {
                        var vals = word.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        WordNode node = new WordNode()
                        {
                            Frequency = vals.Length > 1 ? double.Parse(vals[1]) : 1  //默认词频1
                        };
                        //词
                        mainDict.FillSegment(vals[0].ToLower().ToCharArray(), node);
                    }
                }
            }

            //发射概率实例化
            FinalSeg.Init(Path.Combine(cfg.MainDictionary, "prob_emit.txt"));
        }
        /// <summary>
        /// 是否有效的关键字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool IsValidWord(string word)
        {
            if (!string.IsNullOrEmpty(word))
            {
                string realWord = word.Trim();
                //#，/ 为词库行注释符号
                if (!string.IsNullOrEmpty(realWord) && !realWord.StartsWith("#", StringComparison.Ordinal) && !realWord.StartsWith("/", StringComparison.Ordinal))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
