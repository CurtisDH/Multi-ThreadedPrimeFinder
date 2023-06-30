using System.Collections.Concurrent;
using System.Diagnostics;

namespace Multi_ThreadedPrimeFinder
{
    class PrimeNumberFinder
    {
        private readonly int _endNumber;
        private readonly ConcurrentBag<int> _primeNumbers;
        private readonly ConcurrentBag<Tuple<int, long>> _taskTimes;

        public PrimeNumberFinder(int endNumber)
        {
            _endNumber = endNumber;
            _primeNumbers = new ConcurrentBag<int>();
            _taskTimes = new ConcurrentBag<Tuple<int, long>>();
        }

        public void FindAndReportPrimes()
        {
            Console.WriteLine($"Processor count: {Environment.ProcessorCount}");

            int range = _endNumber / Environment.ProcessorCount;
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                int start = range * i;
                int end = (i == Environment.ProcessorCount - 1) ? _endNumber : range * (i + 1);
                int taskNum = i;

                tasks.Add(Task.Run(() =>
                {
                    FindPrimes(start, end, taskNum);
                    Console.WriteLine(
                        $"Task {taskNum} completed on thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                }));
            }


            Task.WhenAll(tasks).Wait();

            Console.WriteLine($"Total prime numbers found: {_primeNumbers.Count}");

            foreach (var taskTime in _taskTimes.OrderBy(t => t.Item1))
            {
                Console.WriteLine($"Task {taskTime.Item1} took {taskTime.Item2}ms");
            }
        }


        private void FindPrimes(int start, int end, int taskId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = start; i <= end; i++)
            {
                if (IsPrime(i))
                {
                    _primeNumbers.Add(i);
                }
            }

            stopwatch.Stop();
            _taskTimes.Add(Tuple.Create(taskId, stopwatch.ElapsedMilliseconds));
        }

        private bool IsPrime(int number)
        {
            if (number <= 1)
            {
                return false;
            }

            if (number == 2)
            {
                return true;
            }

            if (number % 2 == 0)
            {
                return false;
            }

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void ExportPrimesToFile(string fileName)
        {
            using StreamWriter file = new StreamWriter(fileName);
            foreach (var prime in _primeNumbers.OrderBy(p => p))
            {
                file.WriteLine(prime);
            }
        }
    }
}