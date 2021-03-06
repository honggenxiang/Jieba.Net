﻿using System.Collections.Generic;
using System.Text;
using Jieba.Net.Core.Dict;

namespace Jieba.Net.Core.Core
{
    public class JiebaSegmenter
    {
        /// <summary>
        /// DAG创建
        /// </summary>
        /// <returns>The dag.</returns>
        /// <param name="kw">Kw.</param>
        public Dictionary<int, Dictionary<int, WordNode>> CreateDAG(string kw)
        {
            var trie = Dictionary.GetSingleton();
            var dag = new Dictionary<int, Dictionary<int, WordNode>>();
            var chars = kw.ToCharArray();
            var length = kw.Length;
            int i = 0, j = 0;
            while (i < length)
            {
                var hit = trie.MatchInMainDict(chars, i, j - i + 1);
                if (!hit.IsUnMatch())
                {
                    //匹配
                    if (hit.IsMatch())
                    {
                        if (!dag.ContainsKey(i))
                            dag.Add(i, new Dictionary<int, WordNode>()
                            {
                                [j] = hit.MatchedDictSegment.Node
                            });
                        else
                            dag[i].TryAdd(j, hit.MatchedDictSegment.Node);
                    }
                    //前缀匹配
                    if (hit.IsPrefix())
                    {
                        if (++j < length)
                            continue;
                    }
                }
                else
                {
                    //不匹配添加单字
                    if (!dag.ContainsKey(i))
                    {
                        dag.Add(i, new Dictionary<int, WordNode>(1)
                        {
                            [i] = null
                        });
                    }
                }
                j = ++i;

            }
            return dag;
        }


        /// <summary>
        /// 动态规划计算最优路径
        /// </summary>
        /// <param name="kw"></param>
        /// <param name="dag"></param>
        public Dictionary<int, Pair<int>> Calc(string kw, Dictionary<int, Dictionary<int, WordNode>> dag)
        {
            int length = kw.Length;
            var route = new Dictionary<int, Pair<int>>(length + 1)
            {
                [length] = new Pair<int>(0, 0.0d)
            };
            for (var i = length - 1; i > -1; i--)
            {
                Pair<int> andidate = null;
                foreach (var x in dag[i])
                {
                    var freq = (x.Value?.Frequency).GetValueOrDefault() + route[x.Key + 1].Freq;
                    if (andidate == null)
                        andidate = new Pair<int>(x.Key, freq);
                    else if (freq > andidate.Freq)
                    {
                        andidate.Key = x.Key;
                        andidate.Freq = freq;
                    }
                }
                route.Add(i, andidate);
            }
            return route;
        }

        /// <summary>
        /// 基于词频的最大切分组合
        /// </summary>
        /// <param name="kw"></param>
        /// <returns></returns>
        public List<string> SentenceProcess(string kw)
        {
            var dag = CreateDAG(kw);
            var trie = Dictionary.GetSingleton();
            var route = Calc(kw, dag);
            var tokens = new List<string>();
            int i = 0, length = kw.Length;
            var sb = new StringBuilder();
            while (i < length)
            {
                var key = route[i].Key;
                var kWord = kw.Substring(i, key - i + 1);
                if (i == key)
                {
                    //单字
                    sb.Append(kw[i++]);               
                }
                else
                {
                    if (sb.Length > 1)
                    {
                        var buf = sb.ToString();
                        //待识别的词
                        if (trie.MatchInMainDict(buf.ToCharArray()).IsMatch())
                        {
                            tokens.Add(buf);
                        }else{
                            //todo:
                            //HMM模型识别未登录词
                            //tokens.Add($"Ex_{buf}");
                            tokens.AddRange(FinalSeg.Output(buf));
                        }
                    }
                    if (sb.Length > 0)
                        sb = new StringBuilder();
                    i = key + 1;
                    tokens.Add(kWord);
                }

                //最后一位处理
                if (i == length && sb.Length > 1)
                {
                    var buf = sb.ToString();
                    //待识别的词
                    if (trie.MatchInMainDict(buf.ToCharArray()).IsMatch())
                    {
                        tokens.Add(buf);

                    }
                    else
                    {
                        //todo:
                        //HMM模型识别未登录词
                        //tokens.Add($"Ex_{buf}");
                        tokens.AddRange(FinalSeg.Output(buf));
                    }
                }
            }
            return tokens;
        }
    }
}
