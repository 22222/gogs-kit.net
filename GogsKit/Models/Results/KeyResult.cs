using GogsKit.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace GogsKit
{
    public class KeyResult : JsonEntityBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonIgnore]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAtString
        {
            get { return DateTimeOffsetStringConvert.Serialize(CreatedAt);  }
            set { CreatedAt = DateTimeOffsetStringConvert.Deserialize(value); }
        }
    }
}
