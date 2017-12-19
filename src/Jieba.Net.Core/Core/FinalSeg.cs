// ********************************************
// 作者：honggenxiang
// 时间：2017-12-15 16:25:32
// ********************************************


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jieba.Net.Core.Core
{
    /// <summary>
    /// 1.观测序列；
    /// 2.隐藏状态序列；  
    /// 3.状态初始概率；
    /// 4.状态转移概率；
    /// 5.状态发射概率；
    /// </summary>
    public class FinalSeg
    {
        private static readonly Double minFloat = -3.14e+100;
        /// <summary>
        /// 隐藏状态
        /// </summary>
        public enum State
        {
            /// <summary>
            /// 首字
            /// </summary>
            B,
            /// <summary>
            /// 中间
            /// </summary>
            M,
            /// <summary>
            /// 结尾
            /// </summary>
            E,
            /// <summary>
            /// 单字
            /// </summary>
            S
        }

        private class Node
        {
            /// <summary>
            ///前一状态节点
            /// </summary>
            public Node Pre { get; set; }

            /// <summary>
            /// 当前节点状态
            /// </summary>
            public State Cur { get; set; }

            /// <summary>
            /// 概率
            /// </summary>
            public double Prob { get; set; }

            public override string ToString()
            {
                return $"State:{Cur} Prob:{Prob}";
            }
        }
        /// <summary>
        /// 隐藏状态列表
        /// </summary>
        private static readonly List<State> states = Enum.GetValues(typeof(State)).OfType<State>().ToList();

        /// <summary>
        /// 状态初始概率
        /// </summary>

        private static readonly Dictionary<State, double> probStart = new Dictionary<State, double>
        {
            [State.B] = -0.26268660809250016,
            [State.E] = -3.14e+100,
            [State.M] = -3.14e+100,
            [State.S] = -1.4652633398537678
        };

        /// <summary>
        /// 状态转移概率
        /// </summary>

        private static readonly Dictionary<State, Dictionary<State, double>> probTrans = new Dictionary<State, Dictionary<State, double>>
        {
            [State.B] = new Dictionary<State, double>
            {
                [State.E] = -0.510825623765990,
                [State.M] = -0.916290731874155,
            },
            [State.E] = new Dictionary<State, double>
            {
                [State.B] = -0.5897149736854513,
                [State.S] = -0.8085250474669937
            },
            [State.M] = new Dictionary<State, double>
            {
                [State.E] = -0.33344856811948514,
                [State.M] = -1.2603623820268226
            },
            [State.S] = new Dictionary<State, double>
            {
                [State.B] = -0.26268660809250016,
                [State.S] = -0.26268660809250016
            },
        };

        /// <summary>
        /// 发射概率
        /// </summary>
        private static readonly Dictionary<State, Dictionary<char, double>> probEmit = new Dictionary<State, Dictionary<char, double>>();

        /// <summary>
        /// 初始化HMM模型数据
        /// </summary>
        /// <param name="path"></param>
        internal static void Init(string path)
        {
            var lines = File.ReadAllLines(path);
            var state = default(State);
            foreach (var line in lines)
            {
                if (line.Length == 1)
                {
                    state = Enum.Parse<State>(line);
                    probEmit.Add(state, new Dictionary<char, double>());
                }
                else
                {
                    var sp = line.Split(new[] { '\t' });
                    probEmit[state].Add(sp[0][0], double.Parse(sp[1]));
                }

            }
        }

        private static List<State> Viterbi(string sentence)
        {
            var length = sentence.Length;
            var v = new Dictionary<int, Dictionary<State, Node>>(sentence.Length);
            //初始概率处理
            var v0 = new Dictionary<State, Node>(states.Count);
            foreach (var x in states)
            {
                if (!probEmit[x].TryGetValue(sentence[0], out var emit)) emit = minFloat;
                v0.Add(x, new Node { Pre = null, Cur = x, Prob = probStart[x] + emit });
            }
            v.Add(0, v0);

            for (var i = 1; i < length; i++)
            {
                v[i] = new Dictionary<State, Node>(states.Count);
                foreach (var y in states)
                {
                    //发射概率
                    if (!probEmit[y].TryGetValue(sentence[i], out var emit)) emit = minFloat;
                    Node node = null;
                    //前一个状态
                    foreach (var y0 in states)
                    {
                        if (!probTrans[y0].TryGetValue(y, out var tran)) tran = minFloat;
                        var proby0 = v[i - 1][y0].Prob + tran + emit;
                        if (node == null)
                        {
                            node = new Node { Prob = proby0, Cur = y, Pre = v[i - 1][y0] };
                        }
                        else
                        {
                            if (proby0 > node.Prob)
                            {
                                node.Prob = proby0;
                                node.Pre = v[i - 1][y0];
                            }
                        }
                    }
                    v[i].Add(y, node);
                }

            }
            //找出最大概率
            Node maxNode = null;
            foreach (var kv in v[length - 1])
            {
                if (maxNode == null) maxNode = kv.Value;
                else
                {
                    if (kv.Value.Prob > maxNode.Prob) maxNode = kv.Value;
                }
            }

            var obsStates = new List<State>();
            while (maxNode != null)
            {
                obsStates.Add(maxNode.Cur);
                maxNode = maxNode.Pre;
            }
            return obsStates;
        }

        public static IEnumerable<string> Output(string sentence)
        {
            var tokenStates = Viterbi(sentence);
            var begin = -1;
            for (var i = 0; i < tokenStates.Count; i++)
            {
                var token = tokenStates[i];
                if (token == State.S){
                    begin = -1;
                    yield return sentence.Substring(i, 1);}  //(i, 1);
                else if (token == State.B)
                {
                    begin = i;
                }
                else if (token == State.E && begin > -1)
                {
                    var rs = sentence.Substring(begin, i - begin + 1);//(begin, i - begin + 1);
                    begin = -1;
                    yield return rs;
                }
            }
        }
    }
}