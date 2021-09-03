using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace DebugIssuesApp
{
    public class Test2_ProblemDoNotExists
    {
        private object _lock = new object();

        private void RunThread1()
        {
            new Thread(() =>
            {
                lock (_lock)
                {
                    DateTime nowPlus3s = DateTime.Now.AddSeconds(3);
                    Random random = new Random();

                    while (DateTime.Now <= nowPlus3s)
                    {
                        var startNum = random.Next(1, 10);
                        var endNum = random.Next(1, 10);

                        var operationNum = random.Next(1, 2);
                        int result = 0;

                        if (operationNum == 1)
                            result = startNum + endNum;
                        else
                            result = startNum - endNum;
                    }
                }

            }).Start();
        }

        public void Start()
        {
            RunThread1();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Thread.Sleep(100);
            lock (_lock)
            {
                Console.WriteLine("Inside lock");
            }
            sw.Stop();
            Console.WriteLine($"DONE, {sw.Elapsed.TotalMilliseconds} ms");
        }
    }
}
