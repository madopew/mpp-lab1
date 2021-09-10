using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Core.Tracer.Implementations.TracerResults;
using Core.Tracer.Implementations.Tracers;
using Core.Tracer.Interfaces;
using Main.Serializer.Adapters;
using Main.Serializer.Interfaces;

namespace Main
{
    class Program
    {
        private class Test
        {
            private ITracer tracer;

            public Test(ITracer tracer)
            {
                this.tracer = tracer;
            }

            public void M1()
            {
                tracer.StartTrace();
                Thread.Sleep(100);
                tracer.StopTrace();
            }
            
            public void M2()
            {
                tracer.StartTrace();
                Thread.Sleep(100);
                M1();
                tracer.StopTrace();
            }

            public void M3()
            {
                tracer.StartTrace();
                M1();
                M2();
                tracer.StopTrace();
            }
        }
        static void Main(string[] args)
        {
            var tracer = new Tracer();
            var test = new Test(tracer);

            var t1 = new Thread(() =>
            {
                test.M1();
                test.M2();
                test.M3();
            });
            t1.Start();
            
            var t2 = new Thread(() =>
            {
                test.M1();
                test.M2();
                test.M3();
            });
            t2.Start();
            
            t1.Join();
            t2.Join();

            ISerializer serializer = new JsonSerializer();
            using (var file = new FileStream("C:/Users/Madi/Desktop/result.json", FileMode.Create))
            {
                using (var writer = new StreamWriter(file))
                {
                    serializer.Serialize(writer, tracer.Result);
                }
            }
        }
    }
}