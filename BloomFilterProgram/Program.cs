using System;
using BloomFilterLib;

namespace BloomFilterProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            var bf = new BloomFilter<int>(1000);
            Console.WriteLine("Add 10");
            bf.Add(10);

            Console.WriteLine("Add 500");
            bf.Add(500);

            Console.WriteLine("Add 123");
            bf.Add(123);

            foreach (var e in new int[] { 142143, 53224243, 10, 11, 500, 550, 123, 4321 })
            {
                Console.WriteLine($"{e} is probably in bloom filter: {bf.ProbablyContains(e)}");
            }

        }
    }
}
