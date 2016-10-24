using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace GogsKit
{
    public class CreateUserOption : JsonEntityBase
    {
        [JsonProperty("source_id")]
        public int? SourceId { get; set; }

        [JsonProperty("login_name")]
        public string LoginName { get; set; }

        [JsonProperty("username")]
        [StringLength(35)]
        public string Username { get; set; }

        [JsonProperty("full_name")]
        [StringLength(100)]
        public string FullName { get; set; }

        [JsonProperty("email")]
        [StringLength(254)]
        public string Email { get; set; }

        [JsonProperty("password")]
        [StringLength(255)]
        public string Password { get; set; }

        [JsonProperty("send_notify")]
        public bool SendNotify { get; set; }
    }
}
