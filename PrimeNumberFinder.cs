using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadTesting
{
    static class PrimeNumberFinder
    {
        private static int _endingNum = 1250000;
        private static int _total;

        static void Main(string[] args)
        {
            Console.WriteLine($"Processor count: {Environment.ProcessorCount}");
            EventManager.EventManager.Listen("onThreadComplete",(Action<int>)UpdateTotal);
            CreateThreads(Environment.ProcessorCount);
            Console.ReadLine();
            Console.WriteLine("Total:" + _total);
        }

        static void UpdateTotal(int amount)
        {
            _total += amount;
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
                //Task<int> task = Task<int>.Factory.StartNew(() => FindPrimesTaskInt(endNum - _endingNum, endNum, threadNumber));
                //_total += task.Result;
                Thread thread = new Thread(t => FindPrimesTask(endNum - _endingNum, endNum, threadNumber));
                thread.Start();
            }

            stopwatch.Stop();
            Console.WriteLine($"time since all thread creation:{stopwatch.ElapsedMilliseconds}ms");
            //Console.WriteLine($"time since all thread completion:{stopwatch.ElapsedMilliseconds}ms");
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
            EventManager.EventManager.RaiseEvent("onThreadComplete",_foundPrimes.Count);
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
    
    namespace EventManager
{
    public class EventManager
    {
        private static Dictionary<string, dynamic> _eventDictionary = new Dictionary<string, dynamic>();

        #region Listen
        public static void Listen(string eventName, Action method)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                var eventToAdd = _eventDictionary[eventName];
                eventToAdd += method;
                _eventDictionary[eventName] = eventToAdd;
            }
            else
            {
                _eventDictionary.Add(eventName, method);
            }
        }
        public static void Listen<T>(string eventName, Action<T> method)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                var eventToAdd = _eventDictionary[eventName];
                eventToAdd += method;
                _eventDictionary[eventName] = eventToAdd;
            }
            else
            {
                _eventDictionary.Add(eventName, method);
            }
        }
        public static void Listen<T, Q>(string eventName, Action<T, Q> method)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                var eventToAdd = _eventDictionary[eventName];
                eventToAdd += method;
                _eventDictionary[eventName] = eventToAdd;
            }
            else
            {
                _eventDictionary.Add(eventName, method);
            }
        }
        public static void Listen<T, Q, R>(string eventName, Action<T, Q, R> method)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                var eventToAdd = _eventDictionary[eventName];
                eventToAdd += method;
                _eventDictionary[eventName] = eventToAdd;
            }
            else
            {
                _eventDictionary.Add(eventName, method);
            }
        }
        public static void Listen<T, Q, R, Z>(string eventName, Action<T, Q, R, Z> method)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                var eventToAdd = _eventDictionary[eventName];
                eventToAdd += method;
                _eventDictionary[eventName] = eventToAdd;
            }
            else
            {
                _eventDictionary.Add(eventName, method);
            }
        }
        #endregion
        #region Raising Events

        public static void RaiseEvent(string eventName)
        {
            try
            {
                var EventToRaise = _eventDictionary?[eventName] as Action;
                EventToRaise?.Invoke();
            }
            catch
            {
                //Debug.Log(e.Data);
            }

        }
        public static void RaiseEvent<T>(string eventName, T arg)
        {
            try
            {
                var EventToRaise = _eventDictionary?[eventName] as Action<T>;
                EventToRaise?.Invoke(arg);
            }
            catch
            {

            }

        }
        public static void RaiseEvent<T, Q>(string eventName, T arg, Q arg1)
        {
            try
            {
                var EventToRaise = _eventDictionary?[eventName] as Action<T, Q>;
                EventToRaise?.Invoke(arg, arg1);
            }
            catch
            {

            }

        }
        public static void RaiseEvent<T, Q, R>(string eventName, T arg, Q arg1, R arg2)
        {
            try
            {
                var EventToRaise = _eventDictionary?[eventName] as Action<T, Q, R>;
                EventToRaise?.Invoke(arg, arg1, arg2);
            }
            catch
            {

            }

        }
        public static void RaiseEvent<T, Q, R, Z>(string eventName, T arg, Q arg1, R arg2, Z arg3)
        {
            try
            {
                var EventToRaise = _eventDictionary?[eventName] as Action<T, Q, R, Z>;
                EventToRaise?.Invoke(arg, arg1, arg2, arg3);
            }
            catch
            {

            }

        }
        #endregion
        #region Unsubscribing methods
        public static void UnsubscribeEvent(string eventName, Action method)
        {
            var eventToUnsubscribe = _eventDictionary[eventName];
            eventToUnsubscribe -= method;
            _eventDictionary[eventName] = eventToUnsubscribe;
        }

        public static void UnsubscribeEvent<T>(string eventName, Action<T> method)
        {
            var eventToUnsubscribe = _eventDictionary?[eventName];
            eventToUnsubscribe -= method;
            _eventDictionary[eventName] = eventToUnsubscribe;
        }
        public static void UnsubscribeEvent<T, Q>(string eventName, Action<T, Q> method)
        {
            var eventToUnsubscribe = _eventDictionary[eventName];
            eventToUnsubscribe -= method;
            _eventDictionary[eventName] = eventToUnsubscribe;
        }
        public static void UnsubscribeEvent<T, Q, R>(string eventName, Action<T, Q, R> method)
        {
            var eventToUnsubscribe = _eventDictionary[eventName];
            eventToUnsubscribe -= method;
            _eventDictionary[eventName] = eventToUnsubscribe;
        }
        public static void UnsubscribeEvent<T, Q, R, Z>(string eventName, Action<T, Q, R, Z> method)
        {
            var eventToUnsubscribe = _eventDictionary[eventName];
            eventToUnsubscribe -= method;
            _eventDictionary[eventName] = eventToUnsubscribe;
        }
        
        #endregion

    }
}
    
}