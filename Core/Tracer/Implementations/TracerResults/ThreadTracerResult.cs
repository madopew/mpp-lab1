using System.Collections.Generic;
using Core.Tracer.Interfaces;

namespace Core.Tracer.Implementations.TracerResults
{
    public class ThreadTracerResult : AbstractTracerResult
    {
        public int Id { get; internal set; }
        
        public long ExecutionTime { get; internal set; }
        
        public IReadOnlyList<MethodTracerResult> Methods { get; internal set; }

        public override string ToString()
        {
            return $"{{id: {Id}, time: {ExecutionTime}, methods: [{string.Join(", ", Methods)}]}}";
        }
    }
}