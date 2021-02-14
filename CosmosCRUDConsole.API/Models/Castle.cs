using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosCRUDConsole.API.Models
{
    public class Castle
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
