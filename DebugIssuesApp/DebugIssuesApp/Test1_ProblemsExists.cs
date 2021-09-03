using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DebugIssuesApp
{
    public class Test1_ProblemsExists
    {
        private void SendGetRequest(string url)
        {
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            foreach (var letter in content)
            {
                if (letter == '\0')
                    continue;

                var message = $"{DateTime.Now}, ThreadId {Thread.CurrentThread.ManagedThreadId}, url {url}, char {letter}";

                Debug.WriteLine(message);
                Console.Write(letter);
            }
        }

        private void HttpRequestParameterizedThreadStart(object obj)
        {
            var url = obj as string;
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            while (true)
            {
                foreach (var letter in content)
                {
                    Task.Delay(100).GetAwaiter().GetResult();
                    Debug.WriteLine($"ThreadId {Thread.CurrentThread.ManagedThreadId}: {letter}");
                }
            }
        }

        public void Start(int threadsCount)
        {
            var lines = File.ReadAllLines("urls.txt");
            var threads = new List<Thread>();

            var count = Math.Min(threadsCount, lines.Length);

            foreach (var url in lines.Take(count))
            {
                var thread = new Thread(HttpRequestParameterizedThreadStart);
                threads.Add(thread);
                thread.Start(url);
            }

            Stopwatch sw = new Stopwatch();
            
            sw.Start();
            SendGetRequest(lines[1]);
            sw.Stop();

            Console.WriteLine($"DONE, {sw.Elapsed.TotalMilliseconds} ms");
        }
    }
}
