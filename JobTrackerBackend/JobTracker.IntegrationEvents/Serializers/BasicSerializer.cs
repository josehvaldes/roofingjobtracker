using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.IntegrationEvents.Serializers
{
    public static class BasicSerializer
    {

        public static SerializeResult Serialize<T>(T obj)
        {
            var typeName = typeof(T).FullName ?? string.Empty;
            var content = System.Text.Json.JsonSerializer.Serialize(obj);
            return new SerializeResult(content, typeName);
        }

        public static T? Deserialize<T>(string type, string content)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(content);
        }
    }
}
