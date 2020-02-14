using System;
using Tedd.DictionaryUtils.Tests;

namespace Tedd.DictionaryUtils.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new ToFlatDictionaryTest();
            test.FlattenObject();
        }
    }
}
