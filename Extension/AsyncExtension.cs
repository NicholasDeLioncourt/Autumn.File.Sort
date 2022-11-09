using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.File
{
    //=============================================================================================
    /// <summary>Async extension methods.</summary>
    /// <created>L. Nicholas de Lioncourt</created>
    //=====================================================================================LNDL====
    internal static class AsyncExtension
    {
        public static Task EachAsync<T>(this IEnumerable<T> source, int parallelism, Func<T, Task> body)
        {
            return Task.WhenAll(from __partition in Partitioner.Create(source).GetPartitions(parallelism)
                                select Task.Run(async delegate
                                {
                                    using(__partition) while(__partition.MoveNext())
                                         { await body(__partition.Current); }
                                }));
        }
    }
}
