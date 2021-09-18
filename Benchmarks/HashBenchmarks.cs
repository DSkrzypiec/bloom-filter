using System;
using System.Security.Cryptography;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class HashBenchmarks
    {
        private SHA256 _hashFunc = SHA256.Create();

        [Benchmark]
        public void Sha256Longs()
        {
            var hash = _hashFunc.ComputeHash(BitConverter.GetBytes(1231241244214L));
        }

        [Benchmark]
        public void Sha256Bytes()
        {
            var hash = _hashFunc.ComputeHash(new byte[] { 3, 4, 5, 1, 2, 1, 3, 5, 1, 5 } );
        }
    }
}