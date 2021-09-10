using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core.Tracer.Implementations.TracerResults;
using Core.Tracer.Interfaces;

namespace Core.Tracer.Implementations.Tracers
{
    internal class MethodTracer : ITracer
    {
        private readonly MethodTracerResult tracerResult;
        private readonly int skip;

        public MethodTracer(int skip = 3)
        {
            ExecutorStopwatch = new Stopwatch();
            InnerTracers = new List<MethodTracer>();
            this.skip = skip;
            tracerResult = new MethodTracerResult();
        }

        public bool Active { get; private set; }

        private Stopwatch ExecutorStopwatch { get; }

        private List<MethodTracer> InnerTracers { get; }

        public AbstractTracerResult Result
        {
            get
            {
                tracerResult.Methods = InnerTracers.Select(method => method.Result).Cast<MethodTracerResult>().ToList();
                return tracerResult;
            }
        }

        public void StartTrace()
        {
            if (!Active)
            {
                Active = true;

                var method = new StackFrame(skip).GetMethod();
                tracerResult.MethodName = method.Name;
                tracerResult.ClassName = method.DeclaringType.Name;

                ExecutorStopwatch.Start();
            }
            else
            {
                if (InnerTracers.Any() && InnerTracers.Last().Active)
                {
                    InnerTracers.Last().StartTrace();
                }
                else
                {
                    var tracer = new MethodTracer(skip + 1);
                    InnerTracers.Add(tracer);
                    tracer.StartTrace();
                }
            }
        }

        public void StopTrace()
        {
            if (InnerTracers.Any() && InnerTracers.Last().Active)
            {
                InnerTracers.Last().StopTrace();
            }
            else
            {
                ExecutorStopwatch.Stop();
                Active = false;
                tracerResult.ExecutionTime = ExecutorStopwatch.ElapsedMilliseconds;
            }
        }
    }
}