using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Todo.Models.Gravatar
{
    public class GravatarReponse
    { 

        [JsonPropertyName("entry")]
        public IEnumerable<GravatarProfile> Profiles { get; set;} 
    }
    public class GravatarProfile
    {
        [JsonPropertyName("displayName")]
        public string Name { get; set; }
        [JsonPropertyName("photos")]
        public IEnumerable<GravatarPhoto> Photos { get; set; }
    }

    public class GravatarPhoto
    {
        [JsonPropertyName("value")]
        public string ProfilePicUrl { get; set; }
    }
}
