using System;
using System.Xml.Serialization;
using Core.Tracer.Implementations.TracerResults;

namespace Core.Tracer.Interfaces
{
    [Serializable]
    [XmlInclude(typeof(MethodTracerResult))]
    [XmlInclude(typeof(ThreadTracerResult))]
    [XmlInclude(typeof(TracerResult))]
    public abstract class AbstractTracerResult
    {
    }
}