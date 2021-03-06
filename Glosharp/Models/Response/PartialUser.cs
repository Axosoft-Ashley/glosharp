﻿using Newtonsoft.Json;

namespace Glosharp.Models.Response
{
    public class PartialUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
    }
    
}
