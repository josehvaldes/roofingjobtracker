using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.IntegrationEvents.Serializers
{
    public class SerializeResult
    {
        public string SerializedContent { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public SerializeResult(string serializedContent, string type)
        {
            SerializedContent = serializedContent;
            Type = type;
        }
    }
}
