using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace GogsKit
{
    public class TokenResult : JsonEntityBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }
    }
}
