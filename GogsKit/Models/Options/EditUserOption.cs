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
    public class EditUserOption : JsonEntityBase
    {
        [JsonProperty("source_id")]
        public int? SourceId { get; set; }

        [JsonProperty("login_name")]
        public string LoginName { get; set; }

        [JsonProperty("full_name")]
        [StringLength(100)]
        public string FullName { get; set; }

        [JsonProperty("email")]
        [StringLength(254)]
        public string Email { get; set; }

        [JsonProperty("password")]
        [StringLength(255)]
        public string Password { get; set; }

        [JsonProperty("website")]
        [StringLength(50)]
        public string Website { get; set; }

        [JsonProperty("location")]
        [StringLength(50)]
        public string Location { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        [JsonProperty("admin")]
        public bool? Admin { get; set; }

        [JsonProperty("allow_git_hook")]
        public bool? AllowGitHook { get; set; }

        [JsonProperty("allow_import_local")]
        public bool? AllowImportLocal { get; set; }

        [JsonProperty("max_repo_creation")]
        public int? MaxRepoCreation { get; set; }
    }
}
