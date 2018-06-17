using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TPL
{
    class Program
    {
        private static int[] _nums = Enumerable.Range(0, 100).ToArray();

        static void Main(string[] args)
        {
            //ParallelFors();
            ParallelCancelTokenFor();
        }

        private static void ParallelCancelTokenFor()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            var options = new ParallelOptions
            {
                CancellationToken = tokenSource.Token
            };

            Task.Factory.StartNew(() => 
            {
                Thread.Sleep(3000);
                Console.WriteLine("Cancelling...");
                tokenSource.Cancel();
            });

            try
            {
                Parallel.For(0, 1000, options, i =>
                {
                    Console.WriteLine($"i = {i}, thread = {Thread.CurrentThread.ManagedThreadId}");
                    options.CancellationToken.ThrowIfCancellationRequested();
                    Thread.Sleep(100);
                });
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                tokenSource.Dispose();
            }

            Console.WriteLine("End");
            Console.ReadLine();
        }

        private static void ParallelFors()
        {
            var options = new ParallelOptions
            {
                //MaxDegreeOfParallelism = 2
            };

            var result = Parallel.For(0, 10, options, i =>
            {
                Console.WriteLine($"i = {i}, thread = {Thread.CurrentThread.ManagedThreadId}");

                Thread.Sleep(100);
            });

            Console.ReadLine();

            
            long total = 0;

            Parallel.For<long>(0, _nums.Length, () => 0, (j, loop, subtotal) =>
            {
                subtotal += _nums[j];
                return subtotal;
            },
                x => Interlocked.Add(ref total, x)
            );

            Console.WriteLine($"The total is {total}");
            Console.ReadKey();

            total = 0;

            Parallel.ForEach<int, long>(_nums, () => 0, (j, loop, subtotal) =>
            {
                subtotal += j;
                return subtotal;
            },
            x => Interlocked.Add(ref total, x));

            Console.WriteLine($"The total is {total}");
            Console.ReadKey();
        }
    }
}
