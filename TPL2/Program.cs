using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPL2
{
    class Program
    {
        static void Main(string[] args)
        {
            Parallel.Invoke(
            () =>
                {
                    Console.WriteLine("Task 1");
                },
            () =>
                {
                    Console.WriteLine("Task 2");
                });

            Task<string> task = new Task<string>(() => {
                return "Something";
            });

            task.Start();
            Console.WriteLine($"Task return '{task.Result}'.");

            //------------------------------------

            Task task2 = Task.Run(() => Console.WriteLine("Some task2 output")); //includes definition and Start in one go
            task2.Wait();

            var task3 = Task.Factory.StartNew(() => Console.WriteLine("Start factory"));
            task3.Wait();


            //-------------------------------------

            Task[] taskArray = new Task[10];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) => {
                    Console.WriteLine($"Task {i} started.");
                }, i);
            }
            Task.WaitAll(taskArray);

            //-------------------------------------
            var task4 = Task.Factory.StartNew(() => {
                return "Original task";
            });
            task4.ContinueWith((x) => {
                Console.WriteLine("In continuation: " + x);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            //ContinueWhenAll, ContinueWhenAny

            //--------------------------------------
            var t = Task.Run(() => {
                throw new IndexOutOfRangeException("Some exception thrown.");
            });

            var c = t.ContinueWith((antecedent) =>
            { 
                foreach (var ex in antecedent.Exception.InnerExceptions)
                {
                    if (ex is IndexOutOfRangeException)
                        Console.WriteLine(ex.Message);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            c.Wait();

            //--------------------------------------
            var task1 = Task.Run(() => { throw new FormatException("Throw format exception!"); });

            try
            {
                task1.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    if (e is FormatException)
                    {
                        Console.WriteLine(e.Message);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            //if there are nested task we can use Flatten(), ie. ae.Flatten().InnerExceptions

            //--------------------------------------

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var anotherTask = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);

                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                }
            }, token);

            tokenSource.Cancel();

            try
            {
                anotherTask.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.GetType());
            }

            tokenSource.Dispose();
        }
    }
}
