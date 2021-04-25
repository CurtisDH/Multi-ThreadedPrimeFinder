using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadTesting
{
    static class PrimeNumberFinder
    {
        private static int _endingNum = 500000;
        private static int _total;

        static void Main(string[] args)
        {
            Console.WriteLine($"Processor count: {Environment.ProcessorCount}");
            CreateThreads(Environment.ProcessorCount);
            Console.ReadLine();
            Console.WriteLine("Total:" + _total);
        }

        private static void CreateThreads(int numberOfThreads)
        {
            _endingNum /= numberOfThreads;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 1; i <= numberOfThreads; i++)
            {
                Console.WriteLine(i);
                var endNum = _endingNum * i;
                var threadNumber = i;
                Task<int> task = Task<int>.Factory.StartNew(() => FindPrimesTaskInt(endNum - _endingNum, endNum, threadNumber));
                _total += task.Result;
                //Thread thread = new Thread(t => FindPrimesTask(endNum - _endingNum, endNum, threadNumber));
                //thread.Start();
            }

            stopwatch.Stop();
            //Console.WriteLine($"time since all thread creation:{stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"time since all thread completion:{stopwatch.ElapsedMilliseconds}ms");
        }

        private static void FindPrimesTask(int start, int end,int threadNumber)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool _prime = true;
            List<int> _foundPrimes = new List<int>();
            for (int i = start; i < end; i++)
            {
                _prime = true;
                if (i % 2 == 0)
                {
                    continue;
                }

                for (int t = 2; t <= i / 2; t++)
                {
                    if (i % t == 0)
                    {
                        _prime = false;
                        break;
                    }
                }

                if (_prime)
                {
                    _prime = false;
                    _foundPrimes.Add(i);
                    //NumFound.Add(i);// need to figure out how to return a value safely
                }
            }
            stopwatch.Stop();
            Console.WriteLine(
                $"###THREAD {threadNumber}###Time Elapsed: {stopwatch.ElapsedMilliseconds}ms" +
                $" Found: {_foundPrimes.Count} Started from: {start} Ended at: {end}");
        }
        
        private static int FindPrimesTaskInt(int start, int end,int threadNumber)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool _prime = true;
            List<int> _foundPrimes = new List<int>();
            for (int i = start; i < end; i++)
            {
                _prime = true;
                if (i % 2 == 0)
                {
                    continue;
                }

                for (int t = 2; t <= i / 2; t++)
                {
                    if (i % t == 0)
                    {
                        _prime = false;
                        break;
                    }
                }

                if (_prime)
                {
                    _prime = false;
                    _foundPrimes.Add(i);
                    //NumFound.Add(i);// need to figure out how to return a value safely
                }
            }
            stopwatch.Stop();
            Console.WriteLine(
                $"###THREAD {threadNumber}###Time Elapsed: {stopwatch.ElapsedMilliseconds}ms" +
                $" Found: {_foundPrimes.Count} Started from: {start} Ended at: {end}");
            return _foundPrimes.Count;

        }
        
    }
    
    
    
}