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
    public class CreateTeamOption : JsonEntityBase
    {
        [JsonProperty("name")]
        [StringLength(30)]
        public string Name { get; set; }

        [JsonProperty("description")]
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// read, write or admin
        /// </summary>
        [JsonProperty("permission")]
        public string Permission { get; set; }
    }
}
