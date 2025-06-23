using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DemoWpfApp.Models
{
    public class WebMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public class UserInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }
    }

    public class FormData
    {
        [JsonPropertyName("field1")]
        public string Field1 { get; set; }

        [JsonPropertyName("field2")]
        public string Field2 { get; set; }

        [JsonPropertyName("checkbox")]
        public bool Checkbox { get; set; }
    }
    
}
