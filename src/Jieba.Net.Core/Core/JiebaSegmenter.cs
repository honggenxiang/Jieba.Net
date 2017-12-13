using System.Collections.Generic;
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
        public Dictionary<int, List<int>> CreateDAG(string kw)
        {
            var trie = Dictionary.GetSingleton();
            var dag = new Dictionary<int, List<int>>();
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
                            dag.Add(i, new List<int>() { j });
                        else
                            dag[i].Add(j);
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
                    if (!dag.ContainsKey(i))
                    {
                        dag.Add(i, new List<int>(1) { i });
                    }
                }
                j = ++i;

            }
            return dag;
        }

        public void Calc(string kw, Dictionary<int, List<int>> dag)
        {
            var trie = Dictionary.GetSingleton();
            int length = kw.Length;
            var route = new Dictionary<int, List<int>>(length);
            for (int i = length - 1; i > -1; i--)
            {
                foreach (var x in dag[i])
                {
                    var hit = trie.MatchInMainDict(kw.ToCharArray());
                    if (hit.IsMatch())
                    {
                        if (!route.ContainsKey(x))
                            route.Add(x, new List<int>(1) { hit.MatchedDictSegment.Node.Frequency });
                        else
                        {
                            var max = route[x];
                            //if(max>)
                        }
                    }

                }
            }
        }

    }
}
