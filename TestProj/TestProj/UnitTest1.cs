using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TestProj
{
    public class Tests { 

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            MultiplatformFileWatcher multiplatformFileWatcher;
            multiplatformFileWatcher = new MultiplatformFileWatcher("eventLogs", "*");
            multiplatformFileWatcher.Created += new FileSystemEventHandler((s,e) =>
            {
                Console.WriteLine("CREATED");

                Console.WriteLine(s);
                //var ee = File.ReadAllText("logging.json");

                //var aa = JsonDocument.Parse(ee);

            });
            multiplatformFileWatcher.Changed += new FileSystemEventHandler(async (s, e) =>
            {
                Console.WriteLine("CHANGED");
                try { 
                    var ee = await File.ReadAllTextAsync(e.FullPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                //var jsonn = JsonDocument.Parse(ee);

            });

            Task task = new Task(() => multiplatformFileWatcher.WatchForFileChangesAsync(default(CancellationToken)));
            task.Start();

            int i = 0;

            while (true) {
                if (i > 2)
                {
                    break;
                }

                i++;
                Console.WriteLine("HELLO");
                Trace.Flush();
                Thread.Sleep(5000);
            }
        }
    }
}