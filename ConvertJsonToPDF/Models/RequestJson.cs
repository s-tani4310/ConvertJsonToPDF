using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ConvertJsonToPDF.Models
{
    [DataContract]
    public class RequestJson
    {
        [JsonProperty("data")] public Product[] data { get; set; }

    }
}
