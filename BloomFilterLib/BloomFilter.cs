using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

namespace BloomFilterLib
{
    public interface IBloomFilter<T>
    {
        void Add(T item);
        bool ProbablyContains(T item);
        void SetHashFunctions(IList<Func<byte[], byte[]>> hashFunctions);
    }

    public class BloomFilter<T> : IBloomFilter<T>
    {
        private byte[] _bitset;
        private int _n;
        private int _m;
        private IList<Func<byte[], byte[]>> _hashFuncs;

        public BloomFilter(int m)
        {
            _m = m;
            _bitset = new byte[_m];
            SetDefaultHashFunctions();
        }

        public void Add(T item)
        {
            var bytes = ObjectToByteArray(item);

            for (var i = 0; i < _hashFuncs.Count; i++)
            {
                var idx = Math.Abs(BitConverter.ToInt32(_hashFuncs[i](bytes))) % _m;
                Console.WriteLine("Index - {0}", idx);

                if (_bitset[idx] == 0)
                {
                    _bitset[idx] = 1;
                }
            }
        }

        public bool ProbablyContains(T item)
        {
            var bytes = ObjectToByteArray(item);

            for (var i = 0; i < _hashFuncs.Count; i++)
            {
                var idx = Math.Abs(BitConverter.ToInt32(_hashFuncs[i](bytes))) % _m;

                if (_bitset[idx] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void SetHashFunctions(IList<Func<byte[], byte[]>> hashFunctions)
        {
            _hashFuncs = hashFunctions;
        }

        private byte[] ObjectToByteArray(T item)
        {
            if (item == null)
            {
                return null;
            }

            var bf = new BinaryFormatter(); // TODO: do not use BinaryFormatter
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, item as object);
                return ms.ToArray();
            }
        }

        private void SetDefaultHashFunctions()
        {
            _hashFuncs = new List<Func<byte[], byte[]>>();
            _hashFuncs.Add((byte[] x) => SHA1.HashData(x));
            _hashFuncs.Add((byte[] x) => SHA256.HashData(x));
            _hashFuncs.Add((byte[] x) => SHA512.HashData(x));
        }
    }
}

