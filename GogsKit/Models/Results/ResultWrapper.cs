using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit
{
    internal interface IResultWrapper : IJsonEntity
    {
        bool Ok { get; }
    }

    internal class ErrorResultWrapper : JsonEntityBase, IResultWrapper
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("ok")]
        public bool Ok => false;

    }

    internal interface IDataResultWrapper : IResultWrapper
    {
        object Data { get; }
    }

    internal class DataResultWrapper<T> : JsonEntityBase, IDataResultWrapper where T : class
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonIgnore]
        object IDataResultWrapper.Data => Data;

        [JsonProperty("ok")]
        public bool Ok => true;

        //[JsonProperty("error")]
        //public string Error { get; set; }
    }
}
