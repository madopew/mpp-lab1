using System;
using Main.Serializer.Interfaces;

namespace Main.Serializer.Adapters
{
    public class XmlSerializer : System.Xml.Serialization.XmlSerializer, ISerializer
    {
        public XmlSerializer(Type t)
            : base(t)
        {
            
        }
    }
}