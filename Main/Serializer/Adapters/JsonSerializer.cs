using System.IO;
using Main.Serializer.Interfaces;
using Newtonsoft.Json;

namespace Main.Serializer.Adapters
{
    public class JsonSerializer : ISerializer
    {
        public void Serialize(TextWriter writer, object data)
        {
            writer.WriteLine(
                JsonConvert.SerializeObject(data, Formatting.Indented)
            );
        }
    }
}