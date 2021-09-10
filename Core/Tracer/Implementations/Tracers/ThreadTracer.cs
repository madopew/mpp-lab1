using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Tracer.Implementations.TracerResults;
using Core.Tracer.Interfaces;

namespace Core.Tracer.Implementations.Tracers
{
    internal class ThreadTracer : ITracer
    {
        private readonly ThreadTracerResult tracerResult;

        public ThreadTracer(int threadId)
        {
            InnerTracers = new List<MethodTracer>();
            Active = false;
            tracerResult = new ThreadTracerResult {Id = threadId};
        }

        private List<MethodTracer> InnerTracers { get; }

        private bool Active { get; set; }

        public AbstractTracerResult Result
        {
            get
            {
                tracerResult.ExecutionTime = InnerTracers.Select(method => method.Result as MethodTracerResult).Sum(result => result.ExecutionTime);
                tracerResult.Methods = InnerTracers.Select(method => method.Result as MethodTracerResult).ToList();
                return tracerResult;
            }
        }

        public void StartTrace()
        {
            if (!Active)
            {
                Active = true;
                var methodTracer = new MethodTracer();
                InnerTracers.Add(methodTracer);
                methodTracer.StartTrace();
            }
            else
            {
                InnerTracers.Last().StartTrace();
            }
        }

        public void StopTrace()
        {
            InnerTracers.Last().StopTrace();
            Active = InnerTracers.Last().Active;
        }
    }
}