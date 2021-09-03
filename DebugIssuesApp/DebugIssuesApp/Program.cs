using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace DebugIssuesApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Test1_ProblemsExists().Start(40);
            
            //new Test2_ProblemDoNotExists().Start();

            Console.ReadLine();
        }
    }
}
