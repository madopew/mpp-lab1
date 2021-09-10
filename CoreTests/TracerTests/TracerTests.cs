using System.Threading;
using Core.Tracer.Implementations.TracerResults;
using Core.Tracer.Implementations.Tracers;
using Core.Tracer.Interfaces;
using NUnit.Framework;

namespace CoreTests.TracerTests
{
    [TestFixture]
    public class TracerTests
    {
        private ITracer tracer;
        
        private void M1()
        {
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
        }

        private void M2()
        {
            tracer.StartTrace();
            Thread.Sleep(100);
            M1();
            tracer.StopTrace();
        }

        [SetUp]
        public void Setup()
        {
            tracer = new Tracer();
        }

        [Test]
        public void Trace_SingleMethod_TimeMore100()
        {
            M1();
            Assert.AreEqual(1, (tracer.Result as TracerResult).Threads.Count);
            Assert.GreaterOrEqual((tracer.Result as TracerResult).Threads[0].ExecutionTime, 100);
        }
        
        [Test]
        public void Trace_TwoMethods_TimeMore200_MethodsAmount2()
        {
            M1();
            M1();
            Assert.AreEqual(1, (tracer.Result as TracerResult).Threads.Count);
            Assert.AreEqual(2, (tracer.Result as TracerResult).Threads[0].Methods.Count);
            Assert.GreaterOrEqual((tracer.Result as TracerResult).Threads[0].ExecutionTime, 200);
        }
        
        [Test]
        public void Trace_NestedMethod_TimeMore200_MethodAmount1()
        {
            M2();
            Assert.AreEqual(1, (tracer.Result as TracerResult).Threads.Count);
            Assert.AreEqual(1, (tracer.Result as TracerResult).Threads[0].Methods.Count);
            Assert.GreaterOrEqual((tracer.Result as TracerResult).Threads[0].ExecutionTime, 200);
        }
        
        [Test]
        public void Trace_TwoWithNested_TimeMore300_MethodAmount2()
        {
            M1();
            M2();
            Assert.AreEqual(1, (tracer.Result as TracerResult).Threads.Count);
            Assert.AreEqual(2, (tracer.Result as TracerResult).Threads[0].Methods.Count);
            Assert.GreaterOrEqual((tracer.Result as TracerResult).Threads[0].ExecutionTime, 300);
        }
        
        [Test]
        public void Trace_TwoThreads()
        {
            var t1 = new Thread(M1);
            var t2 = new Thread(M2);
            t1.Start();
            t1.Join();
            t2.Start();
            t2.Join();
            
            Assert.AreEqual(2, (tracer.Result as TracerResult).Threads.Count);
            Assert.AreEqual(1, (tracer.Result as TracerResult).Threads[0].Methods.Count);
            Assert.AreEqual(1, (tracer.Result as TracerResult).Threads[1].Methods.Count);
            Assert.GreaterOrEqual((tracer.Result as TracerResult).Threads[0].ExecutionTime, 100);
            Assert.GreaterOrEqual((tracer.Result as TracerResult).Threads[1].ExecutionTime, 200);
        }
    }
}