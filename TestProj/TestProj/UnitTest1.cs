using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;

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
            multiplatformFileWatcher.Changed += new FileSystemEventHandler((s, e) =>
            {
                Console.WriteLine("CHANGED");
                var ee = File.ReadAllText("logging.json");
                var jsonn = JsonDocument.Parse(ee);

            });

            //var thread = new Thread(() => multiplatformFileWatcher.WatchForFileChangesAsync(default(CancellationToken)));
            //thread.Start();

            //var watcher = new FileSystemWatcher("eventLogs");
            //watcher.Changed += (s, e) =>
            //{
            //    Console.WriteLine("CHANGED");
            //};

            //watcher.Created += (s, e) =>
            //{
            //    Console.WriteLine("CREATED");
            //};


            //while (true)
            //{
            //    for (int i = 0; i < 1000000000; i++)
            //    {
            //        Thread.Sleep(1);
            //    }
            //}



            //var ee = File.ReadAllText("logging.json");

            //var aa = JsonDocument.Parse(ee);

            int i = 0;

            while (true) {
                if (i > 2)
                {
                    break;
                }

                i++;
                //Console.WriteLine("HELLO");
                Trace.Flush();
                Thread.Sleep(5000);
            }
        }
    }
}