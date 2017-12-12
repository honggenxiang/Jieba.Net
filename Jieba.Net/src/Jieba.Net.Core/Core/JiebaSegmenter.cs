using System;
using System.Collections.Generic;
using System.IO;
using Jieba.Net.Core.Dict;

namespace Jieba.Net.Core.Core
{
    public class JiebaSegmenter
    {
        //private  StringReader reader;
        //public JiebaSegmenter(StringReader reader)
        //{
        //    this.reader = reader;
        //}

        public string Next(){

            return string.Empty;
        }

        /// <summary>
        /// DAG创建
        /// </summary>
        /// <returns>The dag.</returns>
        /// <param name="kw">Kw.</param>
        public Dictionary<int, List<int>> CreateDAG(string kw ){
            
            var trie=  Dictionary.GetSingleton();
            var dag = new Dictionary<int, List<int>>();
            var chars = kw.ToCharArray();
            var length = kw.Length;
            int i=0,j = 0;
            while(i<length){
             var hit=   trie.MatchInMainDict(chars, i,j-i+1);
                if(!hit.IsUnMatch()){
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
                        if (++j< length)
                            continue;                    
                    }
                }else{
                    if (!dag.ContainsKey(i)){
                        dag.Add(i, new List<int>(1) { i }); 
                    }
                }
                j = ++i;

            }
            return dag;
        }
    }
}
