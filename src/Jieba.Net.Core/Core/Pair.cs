namespace Jieba.Net.Core.Core
{
    public class Pair<T>
    {
        public T Key { get; set; }

        public double Freq { get; set; }

        public Pair(T key, double freq)
        {
            Key = key;
            Freq = freq;
        }
    }
}