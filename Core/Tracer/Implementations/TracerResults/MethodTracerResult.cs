using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Core.Tracer.Interfaces;

namespace Core.Tracer.Implementations.TracerResults
{
    public class MethodTracerResult : AbstractTracerResult
    {
        public string MethodName { get; internal set; }
        
        public string ClassName { get; internal set; }
        
        public long ExecutionTime { get; internal set; }

        [XmlIgnore]
        public IReadOnlyList<MethodTracerResult> Methods { get; internal set; }

        public override string ToString()
        {
            return $"{{name: {MethodName}, class: {ClassName}, time: {ExecutionTime}, methods: [{string.Join(", ", Methods)}]}}";
        }
    }
}