using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Tracer.Implementations.TracerResults;
using Core.Tracer.Interfaces;

namespace Core.Tracer.Implementations.Tracers
{
    public class Tracer : ITracer
    {
        public Tracer()
        {
            Tracers = new ConcurrentDictionary<int, ThreadTracer>();
        }
        
        private ConcurrentDictionary<int, ThreadTracer> Tracers { get; }
        
        public void StartTrace()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            if (Tracers.ContainsKey(threadId))
            {
                Tracers[threadId].StartTrace();
            }
            else
            {
                var tracer = new ThreadTracer(threadId);
                Tracers.TryAdd(threadId, tracer);
                tracer.StartTrace();
            }
        }

        public void StopTrace()
        {
            Tracers[Thread.CurrentThread.ManagedThreadId].StopTrace();
        }

        public AbstractTracerResult Result => new TracerResult
        {
            Threads = Tracers.Select(idToThread => idToThread.Value.Result as ThreadTracerResult).ToList()
        };
    }
}