using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Core.Tracer.Interfaces;

namespace Core.Tracer.Implementations.TracerResults
{
    public class TracerResult : AbstractTracerResult
    {
        [XmlIgnore]
        public IReadOnlyList<ThreadTracerResult> Threads { get; internal set; }
    }
}